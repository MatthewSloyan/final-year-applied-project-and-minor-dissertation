using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private string json;

    public void writeToDatabase(string json)
    {
        this.json = json;
        Debug.Log(json);
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        // Fixed issue with sending JSON through web request. It seems it can't be sent using a POST, so PUT is required.
        // Code adapted from: https://forum.unity.com/threads/posting-json-through-unitywebrequest.476254/
        UnityWebRequest www = UnityWebRequest.Put("localhost:5000/api/results", json);

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}

