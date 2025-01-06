using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;

    private readonly List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;

    public bool isRageModeActive = false;
    public float rageModeDuration = 10f; // Duration of rage mode in seconds
    public float rageSpeedMultiplier = 2f; // Speed multiplier in rage mode
    public Vector3 rageHeadScale = new Vector3(1.5f, 1.5f, 1f); // Increased head size during rage mode
    private float rageTimer = 0f;
    private float rageProgress = 0f; // Tracks rage bar progress
    public float foodRageIncrement = 0.1f; // How much the bar fills per food
    public UnityEngine.UI.Slider rageBar; // Reference to the slider
    //public bool stopRageIncrement; // Stops accumulating rage while in rage mode


    [SerializeField] private UnityEngine.UI.Image sliderHandle;

    private void Start()
    {
        ResetState();
        rageBar.value = 0;
        rageProgress = 0;
    }

    private void Update()
    {
        if (isRageModeActive)
        {
            rageTimer -= Time.deltaTime;
            if (rageTimer <= 0f)
            {
                DeactivateRageMode();
            }

            StopRageIncrement();
        }

        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                input = Vector2Int.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                input = Vector2Int.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                input = Vector2Int.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {
        // Wait until the next update before proceeding
        if (Time.time < nextUpdate) {
            return;
        }

        // Set the new direction based on the input
        if (input != Vector2Int.zero) {
            direction = input;
        }

        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        // Set the next update time based on the speed
        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segment.gameObject.tag = "SnakeBody"; // Assign the SnakeBody tag
        segments.Add(segment);

        // Increment rage progress
        rageProgress += foodRageIncrement;
        if (rageProgress >= 1f)
        {
            ActivateRageMode();
            rageProgress = 0f; // Reset progress after activation
        }

        // Update rage bar UI
        if (rageBar != null)
        {
            rageBar.value = rageProgress;
        }
    }

    private void StopRageIncrement()
    {
        if (isRageModeActive)
        {
            foodRageIncrement = 0f;
        }
        else { foodRageIncrement = 0.2f; }
    }

    private void DeactivateRageMode()
    {
        isRageModeActive = false;
        speedMultiplier = 1f; // Reset speed
        transform.localScale = Vector3.one; // Reset head size

        // Reset rage bar color
        if (rageBar != null)
        {
            sliderHandle.enabled = true;
            rageBar.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            rageBar.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.white;
            rageBar.value = 0f;
        }
    }

    private void ActivateRageMode()
    {
        isRageModeActive = true;
        rageTimer = rageModeDuration;
        speedMultiplier = rageSpeedMultiplier; // Increase speed
        transform.localScale = rageHeadScale; // Increase head size

        // Change rage bar color
        if (rageBar != null)
        {
            sliderHandle.enabled = false;
            rageBar.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.red;
        }
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;

        // Reset score
        ScoreManager.Instance.ResetScore();

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }


    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y) {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            ScoreManager.Instance.AddScore(10); // Add points when food is eaten
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            HandleGameOver();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls)
            {
                Traverse(other.transform);
            }
            else
            {
                HandleGameOver();
            }
        }

        else if (other.gameObject.CompareTag("SnakeBody") && !isRageModeActive)
        {
            // Ensure the collision check is only for the head of the snake
            if (segments.Count > 0 && other.transform != segments[0])
            {
                HandleGameOver();
            }
        }
    }

    private void HandleGameOver()
    {
        // Trigger the Game Over screen
        ScoreManager.Instance.ShowGameOverScreen();

        // Stop the game
        Time.timeScale = 0f; // Pause the game
    }


    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f) {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        } else if (direction.y != 0f) {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }

        transform.position = position;
    }

    public List<Transform> GetSegments()
    {
        return segments;
    }


}
