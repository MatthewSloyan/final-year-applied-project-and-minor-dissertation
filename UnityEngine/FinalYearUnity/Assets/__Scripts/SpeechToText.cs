//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System.IO;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
using System.Collections;
#endif

public class SpeechToText : MonoBehaviour
{
    #region == Singleton ==
    // Singleton design pattern to get instance of class
    public static SpeechToText Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    #region == Private variables ==
    // Hook up the two properties below with a Text and Button object in your UI.
    [SerializeField]
    private Text outputText;

    // Will be used to swap scenarios on the fly.
    [SerializeField]
    private int selection = 0;
    
    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;
    private int sessionId;
    private int persona;
    private string voiceName;
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
    #endregion

#if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    // Set up and convert speech to text.
    // Code adapted from: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstarts/speech-to-text-from-microphone?tabs=dotnet%2Cx-android%2Clinux%2Candroid&pivots=programming-language-more
    public async void convertSpeechToText(int sessionId, int persona, string voiceName)
    {
        this.sessionId = sessionId;
        this.persona = persona;
        this.voiceName = voiceName;

        // Check if mic permission is granted or if the player is within range of a person.
        if (micPermissionGranted && isPersonActive)
        {
            // Display to the user that the person is listening.
            message = "Listening. Please say something!";

            // Creates an instance of a speech config with specified subscription key and service region.
            // For security this is read in from a text file and is not included on Github. 
            // API_Key.txt is stored in the resources folder of the project.
            // For now just using free trial key.
            //string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

            // Creates an instance of a speech config with specified subscription key and service region.
            //var config = SpeechConfig.FromSubscription("722faee502a24ebeb53cf34a58e22e7d", "westus");
            var config = SpeechConfig.FromSubscription("a2f75e30125b4f099012eaa7f9a5c840", "westeurope");

            // Testing
            //config.SetProperty(PropertyId.Speech_LogFilename, "C:/Users/Matthew/Desktop/Test2.txt");

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

                    // Dispose of recognizer correctly.
                    recognizer.Dispose();
                }
            }
        }
        else
        {
            // Display original message if player moves away.
            message = "Interact with a person to start a chat.";
        }
    }

    // On start up get permission from microphone, and add listener to button.
    void Start()
    {
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
    }

    public int GetSessionID()
    {
        return sessionId;
    }

    public int GetPersona()
    {
        return persona;
    }

    public string GetVoiceName()
    {
        return voiceName;
    }

    // On frame update check for permission from microphone, and if speech is being recognised.
    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "Interact with a person to start a chat.";
        }
#elif PLATFORM_IOS
        if (!micPermissionGranted && Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif

        // Check is listen was success and if there's a message to send.
        if (listenSuccess && messageToSend != "")
        {
            // Implement for now, will use one object.
            AIMLRequest request = new AIMLRequest();
            request.sessionId = sessionId;
            request.persona = persona;
            request.userInput = messageToSend;

            // Send result to client class.
            // Couldn't get this working initial but fixed by adapting the following.
            // https://docs.unity3d.com/ScriptReference/GameObject.AddComponent.html
            Client c = gameObject.AddComponent(typeof(Client)) as Client;
            c.sendText(request, voiceName);

            listenSuccess = false;
            messageToSend = "";
        }

        lock (threadLocker)
        {
            if (outputText != null)
            {
                outputText.text = message;
                TextMeshPro tm = GameObject.Find("DEBUG").gameObject.GetComponent<TextMeshPro>();
                tm.text = message;
            }
        }
    }
}
