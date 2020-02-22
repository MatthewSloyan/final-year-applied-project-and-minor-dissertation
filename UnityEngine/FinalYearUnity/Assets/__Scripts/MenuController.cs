using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region == Private Variables == 
    private static bool isGamePaused = false;
    private GameObject gameOverUI;
    private static GameObject pauseUI;

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
        gameOverUI = GameObject.Find("GameOver_UI");
        gameOverUI.SetActive(false);

        pauseUI = GameObject.Find("PauseMenu");
        pauseUI.SetActive(false);
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
        pauseUI.SetActive(false);

        // Start the game running again
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    // Pauses game if called
    public void PauseGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        pauseUI.transform.position = new Vector3(player.transform.position.x, 1.75f, player.transform.position.z + 3);

        pauseUI.SetActive(true);

        Time.timeScale = 0f;
        isGamePaused = true;
    }
    
    //Displays any UI gameobject.
    public void DisplayUI(GameObject ui)
    {
        gameObject.SetActive(true);
    }

    // Changes UI gameobject
    public void ChangeUI(GameObject newUI)
    {
        gameObject.SetActive(false);
        newUI.SetActive(true);
    }

    // Closes any UI gameobject.
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    // Closes any UI gameobject.
    public void GameOverUI()
    {
        // Turn back on menu.
        gameOverUI.SetActive(true);

        // Get json string from file.
        string json = new ScoreFileManager().LoadScoreFile();

        // Convert string to object. Object returned has the updated score.
        Game game = new Utilities().ToObject<Game>(json);

        Text t = GameObject.Find("TimeGameOver").gameObject.GetComponent<Text>();
        t.text = game.gameTime;

        Text s = GameObject.Find("ScoreGameOver").gameObject.GetComponent<Text>();
        s.text = game.gameScore.ToString();
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
