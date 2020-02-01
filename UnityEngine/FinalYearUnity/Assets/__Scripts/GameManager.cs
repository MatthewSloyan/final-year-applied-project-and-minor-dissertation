using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int playTime = 0;
    private int seconds = 0;
    private int minutes = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncrementTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Code heavily adapted from the following timer tutorial.
    // https://www.youtube.com/watch?v=IQkabxKDY3M
    IEnumerator IncrementTime()
    {
        while (true)
        {
            //waits for 1 seconds.
            yield return new WaitForSeconds(1f);

            playTime++;
            seconds = (playTime % 60);
            minutes = (playTime / 60) % 60;
            
            TextMeshPro tm = GameObject.Find("Time").gameObject.GetComponent<TextMeshPro>();
            tm.text = "Time:\n" + minutes + " Mins " + seconds + " Sec";
        }
    }
}
