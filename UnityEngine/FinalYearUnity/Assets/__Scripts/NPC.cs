using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    private int sessionId;
    private string voiceName;
    //0= rude 1= neutral 2= polite
    private int persona;

    public void SetSessionId()
    {
        sessionId = UnityEngine.Random.Range(1, 10000);
        

    }

    public void SetPersona()
    {
        persona = UnityEngine.Random.Range(0, 3);
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

    public int GetPersona()
    {
        return persona;
    }
}
