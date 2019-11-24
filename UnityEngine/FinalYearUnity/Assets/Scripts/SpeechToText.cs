using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using UnityEngine.Networking;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class SpeechToText : MonoBehaviour
{
    #region == Private Variables == 

    [SerializeField]
    private Text outputText;

    [SerializeField]
    private Button startRecordButton;

    [SerializeField]
    private int selection = 0;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;

    private bool micPermissionGranted = false;

    #endregion

#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    public async void ButtonClick()
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // For security this is read in from a text file and is not included on Github. 
        // API_Key.txt is stored in the root directory of the project
        string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

        var config = SpeechConfig.FromSubscription(API_Key, "westeurope");

        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(config))
        {
            lock (threadLocker)
            {
                waitingForReco = true;
            }

            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            string newMessage = string.Empty;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                newMessage = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }

            lock (threadLocker)
            {
                message = newMessage;
                Debug.Log(message);
                waitingForReco = false;
            }
        }
    }

    public void sendText()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWWForm form = new WWWForm();

        form.AddField("myField", outputText.text);
        //form.AddField("selection", selection);

        Debug.Log(form.data);

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
            TextToSpeech.Instance.ConvertTextToSpeech(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    void Start()
    {
        if (outputText == null)
        {
            Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else if (startRecordButton == null)
        {
            message = "startRecordButton property is null! Assign a UI Button to it.";
            Debug.LogError(message);
        }
        else
        {
            // Continue with normal initialization, Text and Button objects are present.
#if PLATFORM_ANDROID
            // Request to use the microphone, cf.
            // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
            message = "Waiting for mic permission";
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
#elif PLATFORM_IOS
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
            }
#else
            micPermissionGranted = true;
            message = "Click \"Listen\" button to recognize speech";
#endif
            startRecordButton.onClick.AddListener(ButtonClick);
        }
    }

    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif

        lock (threadLocker)
        {
            if (startRecordButton != null)
            {
                startRecordButton.interactable = !waitingForReco && micPermissionGranted;
            }
            if (outputText != null)
            {
                outputText.text = message;
            }
        }
    }

    public class RequestS
    {
        //This method is required by the IComparable
        //interface. 
        public string s;

        public RequestS(string sentence)
        {
            s = sentence;
        }
    }
}
