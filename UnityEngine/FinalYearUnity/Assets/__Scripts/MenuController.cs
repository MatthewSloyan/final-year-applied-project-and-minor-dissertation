using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        pauseMenuUI.SetActive(false);

        // Start the game running again
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    
    #endregion
}
