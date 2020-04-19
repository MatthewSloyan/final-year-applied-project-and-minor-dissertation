using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private AIMLRequest request;
    private string voiceName;

    // Called when listening is a sucess, and sends text to server to be output as audio.
    public void sendText(AIMLRequest request, string voiceName)
    {
        this.request = request;
        this.voiceName = voiceName;

        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        string json = new Utilities().ToJsonString(request);

        //UnityWebRequest www = UnityWebRequest.Put("localhost:5000/request", json);
        //UnityWebRequest www = UnityWebRequest.Post("https://final-year-project-chatbot.herokuapp.com/request", form);
        UnityWebRequest www = UnityWebRequest.Put("http://aaronchannon1.pythonanywhere.com/request", json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string reponse = www.downloadHandler.text;

            try
            {
                Debug.Log("Response" + reponse);
                string[] reponses = reponse.Split('=');

                foreach (string item in reponses)
                {
                    Debug.Log(item);
                }

                TextToSpeech.Instance.ConvertTextToSpeech(reponses[0], voiceName, false);

                new ScoreManager(request.sessionId, Int32.Parse(reponses[1]), request.userInput, reponses[0]).UpdateScore();
            }
            catch
            { 
                TextToSpeech.Instance.ConvertTextToSpeech(reponse, voiceName, false);
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

