using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that generates a NPC Object with its random values
public class NPC : MonoBehaviour
{
    public int sessionId;
    public string voiceName;
    //0= rude 1= neutral 2= polite
    public int persona;

    public bool hasTicket;

    // == Sets ==
    //Randomly sets if the NPC has a ticket or not
    public void SetHasTicket(){

        int rand = UnityEngine.Random.Range(0, 11);

        // Passengers have a 3 in 10 chance of not having a ticket.
        if (rand < 4)
        {
            hasTicket = false;
        }
        else{
            hasTicket = true;
        }
    }

    //Randomly sets the NPC's Session ID
    public void SetSessionId()
    {
        sessionId = UnityEngine.Random.Range(1, 10000);
    }

    //Randomly sets the NPC's persona
    public void SetPersona()
    {
        persona = UnityEngine.Random.Range(0, 3);
    }

    //Randomly sets the voice of the NPC. this determines if the NPC is male or female
    public void SetVoice()
    {
        //0 = male 1= female
        int gender = UnityEngine.Random.Range(0, 2);
        string[] voicesMale = {"en-US-GuyNeural", "en-GB-George-Apollo"};
        string[] voicesFemale = { "en-US-AriaNeural", "de-DE-KatjaNeural" };

        //Sets the male voice
        if (gender == 0)
        {
            int rand = UnityEngine.Random.Range(0, voicesMale.Length);
            voiceName = voicesMale[rand];
        }
        //Sets the female voice
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

    // == Getters ==

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

    public bool GetHasTicket(){
        return hasTicket;
    }
}
