using System.Collections.Generic;

[System.Serializable]
public class Game
{
    public int gameId;
    public List<NPCS> npcs;
}

[System.Serializable]
public class NPCS
{
    public int score;
    public int sessionId;
    public string voiceName;
}
