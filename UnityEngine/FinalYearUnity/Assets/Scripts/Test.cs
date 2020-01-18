//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// <code>
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

public class Test : MonoBehaviour
{
    // Singleton design pattern to get instance of class
    public static Test Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Hook up the two properties below with a Text and Button object in your UI.
    public Text outputText;
    //public Button startRecoButton;

    // Will be used to swap scenarios on the fly.
    [SerializeField]
    private int selection = 0;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;
    private static string messageToSend;

    private static bool listenSuccess = false;
    private bool micPermissionGranted = false;

    // Check if person is active E.g Being looked at.
    private static bool isPersonActive = false;
    public static bool IsPersonActive
    {
        get { return isPersonActive; }
        set { isPersonActive = value; }
    }


#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    public async void ButtonClick()
    {
        // Check if mic permission is granted or if the player is within range of a person.
        if (micPermissionGranted && isPersonActive)
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("722faee502a24ebeb53cf34a58e22e7d", "westus");

            // Make sure to dispose the recognizer after use!
            using (var recognizer = new SpeechRecognizer(config))
            {
                listenSuccess = false;
                messageToSend = "";

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

                    // Set independant variables for sending the message to the server.
                    listenSuccess = true;
                    messageToSend = newMessage;
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
                    waitingForReco = false;
                }
            }
        }
    }

    void Start()
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
        message = "Click button to recognize speech";
#endif
        //startRecoButton.onClick.AddListener(ButtonClick);
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

        if (listenSuccess && messageToSend != "")
        {
            // Send result to client code below, this will be abstracted with time.
            sendText();

            listenSuccess = false;
            messageToSend = "";
        }

        lock (threadLocker)
        {
            //if (startRecoButton != null)
            //{
            //    startRecoButton.interactable = !waitingForReco && micPermissionGranted;
            //}
            if (outputText != null)
            {
                outputText.text = message;
            }
        }
    }

    // Called when listening is a sucess.
    public void sendText()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", messageToSend);
        //outputMessage = "Sending " + messageToSend;
        //form.AddField("selection", selection);

        //Debug.Log(form.data);
        //string jsonStringTrial = JsonUtility.ToJson(form);
        //Debug.Log(jsonStringTrial);

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
            //outputMessage = "Recieved " + www.downloadHandler.text;

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
// </code>
