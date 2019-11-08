using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;

    private object threadLocker = new object();
    private bool waitingForSpeak;
    private string message;

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;

    // Singleton design pattern to get instance of class
    public static TextToSpeech Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ConvertTextToSpeech(string inputText)
    {
        // Code adapted from: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstarts/text-to-speech?tabs=dotnet%2Clinux%2Candroid&pivots=programming-language-csharp
        // Lock the tread while running
        lock (threadLocker)
        {
            waitingForSpeak = true;
        }

        // Creates an instance of a speech config with specified subscription key and service region.
        // For security this is read in from a text file and is not included on Github. 
        // API_Key.txt is stored in the root directory of the project
        string API_Key = System.IO.File.ReadAllText("../../../API_Key.txt");

        speechConfig = SpeechConfig.FromSubscription(API_Key, "westeurope");

        // The default format is Riff16Khz16BitMonoPcm.
        // We are playing the audio in memory as audio clip, which doesn't require riff header.
        // So we need to set the format to Raw16Khz16BitMonoPcm.
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);
        speechConfig.SpeechSynthesisVoiceName = "en-US-JessaNeural";

        // Creates a speech synthesizer.
        // Make sure to dispose the synthesizer after use!
        synthesizer = new SpeechSynthesizer(speechConfig, null);
    }
}
