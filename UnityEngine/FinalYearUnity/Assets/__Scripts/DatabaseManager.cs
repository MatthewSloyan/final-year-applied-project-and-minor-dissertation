using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private string json;

    public void writeToDatabase(string json)
    {
        Debug.Log(json);
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", json);

        //UnityWebRequest www = UnityWebRequest.Post("localhost:5000/request", form);
        //UnityWebRequest www = UnityWebRequest.Post("https://final-year-project-chatbot.herokuapp.com/request", form);
        //UnityWebRequest www = UnityWebRequest.Post("http://aaronchannon1.pythonanywhere.com/request", form);

        using (UnityWebRequest www = UnityWebRequest.Post("localhost:5000/api/result", form))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}

