﻿using System.Collections.Generic;

//Object that holds all data of the game/ training simulation
[System.Serializable]
public class Game
{
    public int gameId;
    public int gameScore;
    public string gameTime;
    public List<NPCList> npcs;
}

// Nested list of class Game, this holds all information about a specific NPC.
[System.Serializable]
public class NPCList
{
    public int sessionId;
    public bool complete;
    public int score;
    public string voiceName;
    public List<string> conversations;
}
