using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartTraining : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Delete player options on load.
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
