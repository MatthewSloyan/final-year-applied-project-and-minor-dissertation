using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    #region == Variables == 
    private static bool isGamePaused = false;

    public static bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    private GameObject pauseMenu;
    #endregion

    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenuWatch");
        pauseMenu.GetComponent<Canvas>().enabled = false;
        //pauseMenu.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("watch"))
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

    // Resumes game if called
    public void ResumeGame()
    {
        Debug.Log("Resume");
        // Turn off the menu UI
        pauseMenu.GetComponent<Canvas>().enabled = false;

        // Start the game running again
        //Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        pauseMenu.GetComponent<Canvas>().enabled = true;

        //Time.timeScale = 0f;
        isGamePaused = true;
    }
}
