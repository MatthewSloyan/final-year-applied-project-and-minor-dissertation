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
    private Button startRecordButton;
   
    private Text outputText;

    // Will be used to swap scenarios on the fly.
    [SerializeField]
    private int selection = 0;

    private object threadLocker = new object();
    private bool waitingForReco;
    private static bool listenSuccess;

    private static string messageToSend;
    private static string outputMessage;
    private static bool micPermissionGranted = false;

    #endregion

    #if PLATFORM_ANDROID || PLATFORM_IOS
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
    #endif
    
    // On button click set up and convert speech to text.
    public async void ButtonClick()
    {
        if (micPermissionGranted)
        {
            // Display to the user that the person is listening.
            outputMessage = "Listening. Please say something!";

            // Creates an instance of a speech config with specified subscription key and service region.
            // For security this is read in from a text file and is not included on Github. 
            // API_Key.txt is stored in the root directory of the project
            string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

            var config = SpeechConfig.FromSubscription(API_Key, "westeurope");
            Debug.Log("Button click1");

            // Create a new instance of a SpeechRecognizer and pass api configuration.
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

                // Checks result, for success, errors or cancellation.
                // If sucess send to server for response.
                string newMessage = string.Empty;
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    newMessage = result.Text;
                    //Debug.Log("Test: " + newMessage);

                    listenSuccess = true;
                    messageToSend = newMessage;
                    //outputText.text = newMessage;
                    //sendText();
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
                    // Message is displayed to user.
                    outputMessage = newMessage;
                    waitingForReco = false;
                }
            }
        }
        else
        {
            outputMessage = "Please allow permission to use the microphone.";
        }
    }

    // On start up get permission from microphone, and add listener to button.
    void Start()
    {
        // Get the UI text.
        outputText = GameObject.Find("DictationText").GetComponent<Text>();

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
            outputMessage = "Interact with a person to chat.";
        #endif

        startRecordButton.onClick.AddListener(ButtonClick);
    }

    // On frame update check for permission from microphone, and if button is pressed record speech.
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

        Debug.Log("Before Success");

        if (listenSuccess)
        {
            Debug.Log(messageToSend);
            sendText();

            Debug.Log("Success");
            listenSuccess = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Clicked");
            ButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Send");
            sendText();
        }

        lock (threadLocker)
        {
            if (startRecordButton != null)
            {
                startRecordButton.interactable = !waitingForReco && micPermissionGranted;
            }
            if (outputText != null)
            {
                outputText.text = outputMessage;
            }
        }
    }

    // Called when 
    public void sendText()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", messageToSend);
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
