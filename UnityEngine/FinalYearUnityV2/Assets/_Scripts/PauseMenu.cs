using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region == Variables == 
    private static bool isGamePaused = false;
    
    public static bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }
    #endregion

    void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
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

    #region == Menu overlays (Pause)

    // Resumes game if called
    public void ResumeGame()
    {
        // Turn off the menu UI
        //gameObject.SetActive(false);
        gameObject.GetComponent<Canvas>().enabled = false;

        // Start the game running again
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        gameObject.GetComponent<Canvas>().enabled = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        gameObject.transform.position = new Vector3(player.transform.position.x, 1.75f, player.transform.position.z + 3);

        Time.timeScale = 0f;
        isGamePaused = true;
    }
    
    #endregion
}
