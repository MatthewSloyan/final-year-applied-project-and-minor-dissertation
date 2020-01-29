﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Net.Http;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private string userInput;
    private int sessionId;

    // Called when listening is a sucess, and sends text to server to be output as audio.
    public void sendText(string userInput,int sessionId)
    {
        this.userInput = userInput;
        this.sessionId = sessionId;
        Debug.Log("NPC's SessionId: " + sessionId);
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", userInput);
        form.AddField("sessionId", sessionId);

        //UnityWebRequest www = UnityWebRequest.Post("localhost:5000/request", form);
        //UnityWebRequest www = UnityWebRequest.Post("https://final-year-project-chatbot.herokuapp.com/request", form);
        UnityWebRequest www = UnityWebRequest.Post("http://aaronchannon1.pythonanywhere.com/request", form);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;

            try
            {
                string[] results = result.Split('=');
                TextToSpeech.Instance.ConvertTextToSpeech(results[0]);

                Debug.Log(results[1]);

                GameObject container = GameObject.Find("container");

                //Array to hold all child obj
                //GameObject[] allChildren = new GameObject[container.transform.childCount];

                // Loop through all children in container.
                // Code adapted from: https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
                foreach (Transform child in container.transform)
                {
                    NPC npc = child.GetComponent<NPC>();

                    Debug.Log(npc.sessionId);

                    if (npc.sessionId == sessionId) {
                        SatisfactionMeter satisfactionMeter = child.GetComponentInChildren<SatisfactionMeter>();

                        Debug.Log("Found");

                        if (results[1] == "0")
                            satisfactionMeter.IncreaseSatifaction();
                        else if (results[1] == "2")
                            satisfactionMeter.DecreaseSatifaction();
                    }
                }
            }
            catch
            {
                TextToSpeech.Instance.ConvertTextToSpeech(result);
            }

            // Send result to TextToSpeech to output audio.
            //TextToSpeech t = gameObject.AddComponent(typeof(TextToSpeech)) as TextToSpeech;
            //t.ConvertTextToSpeech(www.downloadHandler.text);
        }
    }

    //This method is required by the IComparable interface. 
    public class RequestS
    {
        public string s;

        public RequestS(string sentence)
        {
            s = sentence;
        }
    }
}

