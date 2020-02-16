using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    #region == Private Variables == 
    private static bool isGamePaused = false;

    #endregion

    // Singleton design pattern to get instance of class in PlayerCollider.cs
    public static MenuController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get esc key input from keyboard, to pause game from keyboard entry
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    #region == Menu overlays (Pause/Gameover)

    // Resumes game if called
    public void ResumeGame()
    {
        // Turn off the menu UI
        //pauseMenuUI.SetActive(false);

        // Start the game running again
        //Time.timeScale = 1f;
        //isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        //pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        //isGamePaused = true;
    }

    // Display game over screen
    public void GameOverDisplay()
    {
        //gameOverMenuUI.SetActive(true);
    }

    // Displays tutorial menu display.
    //public void DisplayTutorial(GameObject tutorialUI)
    //{
    //    gameObject.SetActive(true);
    //}

    // Changes tutorial page.
    public void ChangeTutorial(GameObject newUI)
    {
        gameObject.SetActive(false);
        newUI.SetActive(true);
    }

    // Closes tutorial menu display.
    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region == Navigation methods == 

    // Resets game
    public void ResetGame()
    {
        // Reload current scene abd un pause game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Quit the game.
    // Code adapted from: https://docs.unity3d.com/ScriptReference/Application.Quit.html
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
