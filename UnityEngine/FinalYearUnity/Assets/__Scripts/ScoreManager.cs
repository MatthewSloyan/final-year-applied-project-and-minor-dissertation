using System;
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
    private static bool callOnce = false;

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
        // Test
        scoreValue = 3;

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
                    child.GetComponentInChildren<CompleteRing>().changeRingColour("Green");
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

        // Update gameTime
        game.gameTime = GameObject.Find("Time").gameObject.GetComponent<TextMeshPro>().text;

        // Create a new list to update and overwrite current list.
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
                else if (scoreValue == 3)
                {
                    obj.score++;
                    obj.complete = true;
                    Debug.Log(obj.complete);
                }
                break;
            }
        }

        game.npcs = list;

        // Write back out to file.
        json = new Utilities().ToJsonString(game);
        scoreFileManager.WriteScoreFile(json);

        // Check if game is complete, E.g if all tickets have been checked.
        bool isGameComplete = CheckGameComplete(game.npcs);

        if (isGameComplete)
        {
            Debug.Log("Game Complete!");
            
            GameObject.Find("ColStick").GetComponent<PersonCollider>().GameComplete();

            if (!callOnce)
            {
                callOnce = true;

                // Display end game menu.
                GameObject.Find("GameOver_UI").gameObject.GetComponent<GameOverMenu>().GameOverUI();
                
                // Write to Database.
                Debug.Log("Test");
                GameObject.Find("Plane").GetComponent<DatabaseManager>().writeToDatabase(json);
            }
        }
    }
    
    private Game UpdateGameScore(Game game)
    {
        if (scoreValue == 0)
        {
            game.gameScore--;
        }
        else if (scoreValue == 2)
        {
            game.gameScore++;
        }

        // Update in game board.
        TextMeshPro tm = GameObject.Find("Score").gameObject.GetComponent<TextMeshPro>();
        tm.text = "Score: " + game.gameScore;

        return game;
    }

    private bool CheckGameComplete(List<NPCList> npcs)
    {
        bool isGameComplete = true;

        // Loop through list of NPCS to check if game is complete.
        foreach (NPCList obj in npcs)
        {
            if (obj.complete == false)
            {
                isGameComplete = false;
                break;
            }
        }

        return isGameComplete;
    }
}
