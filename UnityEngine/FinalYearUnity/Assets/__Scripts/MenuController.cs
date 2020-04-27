using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region == Menu overlays ==

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
    #endregion

    #region == Navigation methods == 

    // Resets game
    public void ResetGame()
    {
        // Reload current scene abd un pause game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit the game.
    // Code adapted from: https://docs.unity3d.com/ScriptReference/Application.Quit.html
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
