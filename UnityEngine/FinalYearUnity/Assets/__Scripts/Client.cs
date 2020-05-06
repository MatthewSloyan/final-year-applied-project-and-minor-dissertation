using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

//Class that makes a connection to the Flask server and receives a response
public class Client : MonoBehaviour
{
    //Request object that holds necessary data
    private AIMLRequest request;

    // Called when listening is a sucess, and sends text to server to be output as audio.
    public void sendText(AIMLRequest request)
    {
        this.request = request;

        StartCoroutine(GetText());
    }

    //CoRoutine that gets the prediction from the flask server
    IEnumerator GetText()
    {   
        //Parses data to JSON
        string json = new Utilities().ToJsonString(request);

        //Makes connection to the flask server on PythonAnywhere and sends the JSON data
        UnityWebRequest www = UnityWebRequest.Put("http://aaronchannon1.pythonanywhere.com/request", json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        //Displays error
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        //Waits for the response from the server and passes it to the score manager
        else
        {
            string reponse = www.downloadHandler.text;

            string[] reponses = reponse.Split('=');

            // If response has a score value E.g 3 to complete NPC.
            if (reponses.Length > 1){
                TextToSpeech.Instance.ConvertTextToSpeech(reponses[0], request.voiceName, false);
                new ScoreManager(request.sessionId, Int32.Parse(reponses[1]), request.userInput, reponses[0]).UpdateScore();
            }
            else {
                TextToSpeech.Instance.ConvertTextToSpeech(reponse, request.voiceName, false);
                new ScoreManager(request.sessionId, 1, request.userInput, reponse).UpdateScore();
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

