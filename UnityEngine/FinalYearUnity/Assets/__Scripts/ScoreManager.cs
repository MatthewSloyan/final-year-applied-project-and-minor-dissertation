using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Test method, will be implemented properly.
    private void UpdateScore(int scoreValue)
    {
        ScoreFileManager scoreFileManager = new ScoreFileManager();

        string json = scoreFileManager.LoadScoreFile();
        Game game = new Utilities().ToObject<Game>(json);

        //List<NPCS> list = new List<NPCS>();
        //list = game.npcs;

        // Loop through list of NPCS to find correct one and update score.
        foreach (NPCS obj in game.npcs)
        {
            if (obj.sessionId == 124)
            {
                if (scoreValue == 1)
                    obj.score++;
                else
                    obj.score--;
                break;
            }
        }
        
        //game.npcs = list;

        // Write back out to file.
        scoreFileManager.WriteScoreFile(new Utilities().ToJsonString(game));
    }
}
