using System.Collections.Generic;

[System.Serializable]
public class Game
{
    public int gameId;
    public int gameScore;
    public string gameTime;
    public List<NPCS> npcs;
}

[System.Serializable]
public class NPCS
{
    public int sessionId;
    public int score;
    public string voiceName;
    public List<string> conversation;
}
