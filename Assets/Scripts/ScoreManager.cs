using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton for global access
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public TextMeshProUGUI highScoreText; // Assign in Inspector for in-game display

    public TextMeshProUGUI finalScoreText; // Assign in Inspector for final score

    public GameObject gameOverPanel; // Assign the Game Over UI panel in Inspector


    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        } 

        // Ensure a single instance of ScoreManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the saved high score from PlayerPrefs
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    public void AddScore(int points)
    {
        Score += points;

        // Update high score if needed
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }

        // Update in-game high score text
        UpdateHighScoreUI();
    }

    public void ResetScore()
    {
        Score = 0;
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {HighScore}";
        }
    }

    public void ShowGameOverScreen()
    {
        // Update the final score and high score text
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {Score}\nHigh Score: {HighScore}";
        }

        // Activate the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        gameOverPanel.SetActive(false); // Hide Game Over UI
        ResetScore(); // Reset the score

        // Start a coroutine to wait and then reset the Snake
        StartCoroutine(ResetSnakeAfterDelay());
    }

    private IEnumerator ResetSnakeAfterDelay()
    {
        // Wait for a brief moment to ensure the Snake object is reloaded
        yield return new WaitForSeconds(0.1f); 

        // Now find and reset the Snake object
        Snake snake = FindObjectOfType<Snake>();
        if (snake != null)
        {
            snake.ResetState(); // Reset the snake's position and segments
        }
    }
}
