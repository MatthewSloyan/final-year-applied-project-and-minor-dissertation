using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    private int sessionId;
    private string voiceName;

    public void SetSessionId()
    {
        sessionId = UnityEngine.Random.Range(1, 10000);
    }

    public void SetVoice()
    {
        string[] voices = { "en-US-JessaNeural", "en-US-GuyNeural", "en-IE-Sean", "de-DE-KatjaNeural" };

        int rand = UnityEngine.Random.Range(1, voices.Length);
        voiceName = voices[rand];
    }

    public int GetSessionID()
    {
        return sessionId;
    }

    public string GetVoiceName()
    {
        return voiceName;
    }
}
