using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public int sessionId;
    public string voiceName;
    //0= rude 1= neutral 2= polite
    public int persona;

    void Start()
    {
        SetSessionId();
        SetPersona();
        SetVoice();
    }

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
        //0 = male 1= female
        int gender = UnityEngine.Random.Range(0, 1);
        Debug.Log("GENDER:" + gender);
        string[] voicesMale = {"en-US-GuyNeural", "en-IE-Sean"};
        string[] voicesFemale = { "en-US-JessaNeural", "de-DE-KatjaNeural" };

        if (gender == 0)
        {
            int rand = UnityEngine.Random.Range(0, voicesMale.Length);
            voiceName = voicesMale[rand];
        }
        else if(gender == 1)
        {
            int rand = UnityEngine.Random.Range(0, voicesFemale.Length);
            voiceName = voicesFemale[rand];
        }

    }

    public void SetVoice(string voice)
    {
        voiceName = voice;

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
