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
    private static bool micPermissionGranted = false;

    private static string messageToSend;
    private static string outputMessage;

    // Check if person is active E.g Being looked at.
    private static bool isPersonActive = false;
    public static bool IsPersonActive
    {
        get { return isPersonActive; }
        set { isPersonActive = value; }
    }

    //private Client client;

    #endregion

    #if PLATFORM_ANDROID || PLATFORM_IOS
        // Required to manifest microphone permission, cf.
        // https://docs.unity3d.com/Manual/android-manifest.html
        private Microphone mic;
        private string message;
    #endif

    // On button click set up and convert speech to text.
    // Code adapted from: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstarts/speech-to-text-from-microphone?tabs=dotnet%2Cx-android%2Clinux%2Candroid&pivots=programming-language-more
    public async void ButtonClick()
    {
        // Check if mic permission is granted or if the player is within range of a person.
        if (micPermissionGranted && isPersonActive)
        {
            // Display to the user that the person is listening.
            outputMessage = "Listening. Please say something!";

            // Creates an instance of a speech config with specified subscription key and service region.
            // For security this is read in from a text file and is not included on Github. 
            // API_Key.txt is stored in the root directory of the project
            string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

            var config = SpeechConfig.FromSubscription(API_Key, "westeurope");

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
                string newMessage = string.Empty;
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    newMessage = result.Text;

                    // Set independant variables for sending the message to the server.
                    listenSuccess = true;
                    messageToSend = newMessage;

                    // Tried to call sendText() here but couldn't get it working.
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
            outputMessage = "Please allow permission to use the microphone, or move closer to person.";
        }
    }

    // On start up get permission from microphone, and add listener to button.
    void Start()
    {
        //client = new Client();

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
        
        // Check 
        if (listenSuccess && messageToSend != "")
        {
            // Send result to client code below, this will be abstracted with time.
            sendText();
            //client.sendText(messageToSend);

            listenSuccess = false;
            messageToSend = "";
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Debug.Log("Clicked");
        //    ButtonClick();
        //}

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    Debug.Log("Send");
        //    client.sendText(messageToSend);
        //}

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

    // Called when listening is a sucess.
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
