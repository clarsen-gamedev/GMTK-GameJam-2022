// Title: GameManager.cs
// Author: Connor Larsen
// Date: 07/15/2022
// Description: Contains functions which control various aspects of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Public and Serialized Variables
    [Header("Gameplay Objects")]
    public GameObject player;     // Reference to the player object in the scene
    public Vector3 startPosition; // Starting position for the player on game start

    [Header("Roulette Colliders")]
    public GameObject[] ring1Colliders;

    [Header("Animations")]
    public Animator gateAnimator;

    [Header("UI Elements")]
    [SerializeField] GameObject gameplayUI; // UI screen for gameplay
    [SerializeField] GameObject pauseUI;    // UI screen for pause screen
    [SerializeField] GameObject gameOverUI; // UI screen for game over screen
    [SerializeField] Text gameOverText;     // Text for the game over screen

    [Header("Controls")]
    [SerializeField] KeyCode pauseButton = KeyCode.Escape;  // Reference to the key responsible for pausing the game

    // Hidden from Inspector
    [HideInInspector] public enum UIScreens { GAME, PAUSE, GAMEOVER, MAX };
    #endregion

    #region Private Variables
    private bool isPaused = false;
    #endregion

    #region Functions
    // Awake is called on the first possible frame
    private void Awake()
    {
        ResetGame();
    }

    // Update is called once per frame
    private void Update()
    {
        // Pause the game
        if (Input.GetKeyDown(pauseButton))
        {
            if (isPaused)   // If game is paused...
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Game Over Screen Message
        if (player.GetComponent<CheckDieValue>().currentSide == 1)
        {
            gameOverText.text = "Oof";
        }

        else if (player.GetComponent<CheckDieValue>().currentSide == 2)
        {
            gameOverText.text = "Big oof";
        }

        else if (player.GetComponent<CheckDieValue>().currentSide == 3)
        {
            gameOverText.text = "Git gud scrub";
        }

        else if (player.GetComponent<CheckDieValue>().currentSide == 4)
        {
            gameOverText.text = "Could have been better";
        }

        else if (player.GetComponent<CheckDieValue>().currentSide == 5)
        {
            gameOverText.text = "You call that a roll?";
        }

        else
        {
            gameOverText.text = "By all accounts, that was bad";
        }
    }

    // Call this function to unpause the game
    public void ResumeGame()
    {
        UISwitch(UIScreens.GAME);   // Switch to game screen
        Time.timeScale = 1f;        // Resume time
        isPaused = false;           // Resume game
        Debug.Log("GAME RESUMED");
    }

    // Call this function to pause the game
    public void PauseGame()
    {
        UISwitch(UIScreens.PAUSE);  // Switch to pause screen
        Time.timeScale = 0f;        // Pause time
        isPaused = true;            // Pause game
        Debug.Log("GAME PAUSED");
    }

    // Call this function to reinitialize the game
    public void ResetGame()
    {
        // Reset the UI
        UISwitch(UIScreens.GAME);   // Switch screen
        Time.timeScale = 1f;        // Resume time
        isPaused = false;           // Resume game

        // Reset the player
        player.transform.position = startPosition;                  // Reset player position
        player.transform.rotation = Quaternion.identity;            // Reset player rotation
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;   // Reset player movement

        // Reset Roulette Colliders
        foreach (GameObject collider in ring1Colliders)
        {
            collider.GetComponent<RouletteValueCheck>().isActive = false;   // Set all colliders to inactive
            collider.SetActive(true);
        }
        ring1Colliders[Random.Range(0, ring1Colliders.Length)].GetComponent<RouletteValueCheck>().isActive = true;  // Activate a random collider

        // Reset Animations
        gateAnimator.Play("GateClose");
    }

    // Call this function when the player dies
    public void GameOver()
    {
        Time.timeScale = 0f;            // Pause time
        UISwitch(UIScreens.GAMEOVER);   // Switch to the Game Over Screen
    }

    // Call this function to quit the game
    public void QuitGame()
    {
        Application.Quit(); // Exit the game
    }

    // Function for enabling and disabling specific UI screens
    public void UISwitch(UIScreens screen)
    {
        if (screen == UIScreens.GAME)
        {
            gameplayUI.SetActive(true);
            pauseUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        else if (screen == UIScreens.PAUSE)
        {
            gameplayUI.SetActive(false);
            pauseUI.SetActive(true);
            gameOverUI.SetActive(false);
        }

        else if (screen == UIScreens.GAMEOVER)
        {
            gameplayUI.SetActive(false);
            pauseUI.SetActive(false);
            gameOverUI.SetActive(true);
        }
    }
    #endregion
}