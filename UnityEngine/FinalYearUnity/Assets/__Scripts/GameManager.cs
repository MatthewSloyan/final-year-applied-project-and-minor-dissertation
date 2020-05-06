using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This Class keeps track of the time for the Training simulation
public class GameManager : MonoBehaviour
{
    private int playTime = 0;
    private int seconds = 0;
    private int minutes = 0;

    private TextMeshPro tm;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncrementTime());

        // Find watch text to update.
        tm = GameObject.Find("Time").gameObject.GetComponent<TextMeshPro>();
    }

    // Increment the in game time, and display on watch.
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
            
            tm.text = "Time:\n" + minutes + " Mins " + seconds + " Sec";
        }
    }
}
