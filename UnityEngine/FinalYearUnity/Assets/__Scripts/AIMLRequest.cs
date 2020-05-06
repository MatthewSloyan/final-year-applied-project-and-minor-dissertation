[System.Serializable]
//Object that holds necessary values to be sent to the Flask server
public class AIMLRequest
{
    public int sessionId;
    public int persona;
    public string voiceName;
    public string userInput;
    public bool hasTicket;
}
