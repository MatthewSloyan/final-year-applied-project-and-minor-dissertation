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

    public class RequestS
    {
        public string s;

        public RequestS(string sentence)
        {
            s = sentence;
        }

        //This method is required by the IComparable
        //interface. 
    }

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        //Here you add 3 BadGuys to the List
        WWWForm form = new WWWForm();
        form.AddField("myField", "hello");

        string jsonStringTrial = JsonUtility.ToJson(form);

        Debug.Log(jsonStringTrial);

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

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

}

