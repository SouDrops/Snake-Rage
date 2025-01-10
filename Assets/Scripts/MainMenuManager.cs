using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class　MainMenuManager : MonoBehaviour
{
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public GameObject backButton;

    private void Start()
    {
        // Ensure all buttons are initially enabled
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
        if (optionsButton != null)
        {
            optionsButton.SetActive(true);
        }
        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }
        // Ensure the Back button is initially disabled
        if (backButton != null)
        {
            backButton.SetActive(false);
        }
    }

    // Function to handle Options button press
    public void OnOptionsButtonClicked()
    {
        // Hide Start, Options, and Quit buttons
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
        if (optionsButton != null)
        {
            optionsButton.SetActive(false);
        }
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }

        // Show the Back button
        if (backButton != null)
        {
            backButton.SetActive(true);
        }

        Debug.Log("Options button clicked!");
    }

    // Function to handle Back button press
    public void OnBackButtonClicked()
    {
        // Show Start, Options, and Quit buttons
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
        if (optionsButton != null)
        {
            optionsButton.SetActive(true);
        }
        if (quitButton != null)
        {
            quitButton.SetActive(true);
        }

        // Hide the Back button
        if (backButton != null)
        {
            backButton.SetActive(false);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Snake");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!"); // Logs for testing in the Editor
        Application.Quit(); 
    }
}
