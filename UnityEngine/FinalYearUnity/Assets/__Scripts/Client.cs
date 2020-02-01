using System;
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
    private string voiceName;

    // Called when listening is a sucess, and sends text to server to be output as audio.
    public void sendText(string userInput,int sessionId, string voiceName)
    {
        this.userInput = userInput;
        this.sessionId = sessionId;
        this.voiceName = voiceName;
        Debug.Log("NPC's Voice: " + voiceName);
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

                TextToSpeech.Instance.ConvertTextToSpeech(results[0], voiceName);

                new ScoreManager(sessionId, Int32.Parse(results[1])).UpdateScore();
            }
            catch
            {
                TextToSpeech.Instance.ConvertTextToSpeech(result, voiceName);
            }
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

