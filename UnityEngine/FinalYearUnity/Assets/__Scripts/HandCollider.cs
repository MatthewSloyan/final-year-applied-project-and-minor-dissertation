using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that handles if the user touches hand to pause the simulation
public class HandCollider : MonoBehaviour
{
    #region == Variables == 
    private static bool isGamePaused = false;

    public static bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    public GameObject pauseMenu;
    #endregion

    void Start()
    {
        //pauseMenu.GetComponent<Canvas>().enabled = false;
        pauseMenu.SetActive(false); 
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
        // Turn off the menu UI
        //pauseMenu.GetComponent<Canvas>().enabled = false;
        pauseMenu.SetActive(false); 

        // Start the game running again
        isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        pauseMenu.SetActive(true); 
        //pauseMenu.GetComponent<Canvas>().enabled = true;

        isGamePaused = true;
    }
}
