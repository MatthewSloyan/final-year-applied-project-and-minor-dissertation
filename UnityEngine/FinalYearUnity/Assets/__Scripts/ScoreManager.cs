﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager
{
    private int sessionId;
    private int scoreValue;
    private string userInput;
    private string npcResponse;
    
    public ScoreManager(int sessionId, int scoreValue, string userInput, string npcResponse)
    {
        this.sessionId = sessionId;
        this.scoreValue = scoreValue;
        this.userInput = userInput;
        this.npcResponse = npcResponse;
    }

    // Test method, will be implemented properly.
    public void UpdateScore()
    {
        UpdateGameObjects();
        UpdateScoreFile();
    }
    
    private void UpdateGameObjects()
    {
        //scoreValue = 3;

        // == Update Satisfaction meter ==
        GameObject container = GameObject.Find("container");

        // Loop through all children in container.
        // Code adapted from: https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
        foreach (Transform child in container.transform)
        {
            NPC npc = child.GetComponent<NPC>();

            if (npc.GetSessionID() == sessionId)
            {
                // If ticket has been checked then set ring to green below NPC
                if (scoreValue == 3)
                {
                    child.GetComponentInChildren<CompleteRing>().ticketChecked();
                    break;
                }

                SatisfactionMeter satisfactionMeter = child.GetComponentInChildren<SatisfactionMeter>();

                if (scoreValue == 0)
                    satisfactionMeter.DecreaseSatifaction();
                else if (scoreValue == 2)
                    satisfactionMeter.IncreaseSatifaction();
            }
        }
    }

    private void UpdateScoreFile()
    {
        // == Update score file ==
        ScoreFileManager scoreFileManager = new ScoreFileManager();

        // Get json string from file.
        string json = scoreFileManager.LoadScoreFile();

        // Convert string to object. Object returned has the updated score.
        Game game = UpdateGameScore(new Utilities().ToObject<Game>(json));

        List<NPCList> list = new List<NPCList>();
        list = game.npcs;

        // Loop through list of NPCS to find correct one and update score.
        foreach (NPCList obj in list)
        {
            if (obj.sessionId == sessionId)
            {
                // Add conversation to existing list in object.
                obj.conversations.Add(userInput);
                obj.conversations.Add(npcResponse);

                Debug.Log(userInput);

                // Increment or decrement score.
                if (scoreValue == 0)
                {
                    obj.score--;
                    Debug.Log(obj.score);
                }
                else if (scoreValue == 2)
                {
                    obj.score++;
                    Debug.Log(obj.score);
                }
                break;
            }
        }

        game.npcs = list;

        // Write back out to file.
        scoreFileManager.WriteScoreFile(new Utilities().ToJsonString(game));
    }

    private Game UpdateGameScore(Game game)
    {
        if (scoreValue == 0)
            game.gameScore--;
        else if (scoreValue == 2)
            game.gameScore++;

        // Update in game board.
        TextMeshPro tm = GameObject.Find("Score").gameObject.GetComponent<TextMeshPro>();
        tm.text = "Score: " + game.gameScore;

        return game;
    }
}
