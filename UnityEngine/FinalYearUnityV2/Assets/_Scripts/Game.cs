using System.Collections.Generic;

[System.Serializable]
public class Game
{
    public int gameId;
    public int gameScore;
    public string gameTime;
    public List<NPCList> npcs;
}

[System.Serializable]
public class NPCList
{
    public int sessionId;
    public bool complete;
    public int score;
    public string voiceName;
    public List<string> conversations;
}
