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

    // Called when listening is a sucess, and sends text to server to be output as audio.
    public void sendText(string userInput)
    {
        this.userInput = userInput;
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", userInput);

        //UnityWebRequest www = UnityWebRequest.Post("localhost:5000", form);
        UnityWebRequest www = UnityWebRequest.Post("https://final-year-project-chatbot.herokuapp.com/request", form);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Send result to TextToSpeech to output audio.
            TextToSpeech.Instance.ConvertTextToSpeech(www.downloadHandler.text);

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

