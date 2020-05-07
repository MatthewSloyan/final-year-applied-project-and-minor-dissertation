using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartTraining : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
