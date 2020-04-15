using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartTraining : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();

        // if (OVRInput.GetDown(OVRInput.Button.One)){
        //     SceneManager.LoadScene(1);
        // }
    }

    void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
