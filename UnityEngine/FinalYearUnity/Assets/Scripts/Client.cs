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
    // Singleton design pattern to get instance of class
    //public static Client Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //}

    // Called when 
    public void sendText(string userInput)
    {
        StartCoroutine(GetText(userInput));
    }

    IEnumerator GetText(string userInput)
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", userInput);
        //form.AddField("selection", selection);

        //Debug.Log(form.data);
        //string jsonStringTrial = JsonUtility.ToJson(form);
        //Debug.Log(jsonStringTrial);

        UnityWebRequest www = UnityWebRequest.Post("localhost:5000", form);
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

            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
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

