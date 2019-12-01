using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    #region == Private Variables == 
    private AudioSource audioSource;
    private AudioClip audioClip;

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;

    private SpeechToText speech;
    #endregion

    // Singleton design pattern to get instance of class
    public static TextToSpeech Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        speech = new SpeechToText();
    }

    public void ConvertTextToSpeech(string inputText)
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // For security this is read in from a text file and is not included on Github. 
        // API_Key.txt is stored in the root directory of the project
        string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

        speechConfig = SpeechConfig.FromSubscription(API_Key, "westeurope");

        // The default format is Riff16Khz16BitMonoPcm.
        // We are playing the audio in memory as audio clip, which doesn't require riff header.
        // So we need to set the format to Raw16Khz16BitMonoPcm.
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);
        speechConfig.SpeechSynthesisVoiceName = "en-US-JessaNeural";

        // Creates a speech synthesizer.
        // Make sure to dispose the synthesizer after use!
        synthesizer = new SpeechSynthesizer(speechConfig, null);

        // Starts speech synthesis, and returns after a single utterance is synthesized.
        using (var result = synthesizer.SpeakTextAsync(inputText).Result)
        {
            // Checks result of synthesizer.
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                // Native playback is not supported on Unity yet (currently only supported on Windows/Linux Desktop).
                // Use the Unity API to play audio by looping through the bytes.
                // Code adapted from: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/quickstarts/text-to-speech?tabs=dotnet%2Clinux%2Candroid&pivots=programming-language-csharp
                var sampleCount = result.AudioData.Length / 2;
                var audioData = new float[sampleCount];
                for (var i = 0; i < sampleCount; ++i)
                {
                    audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
                }

                // Create an AudioClip using the audioData
                // The output audio format is 16K 16bit mono
                audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                audioClip.SetData(audioData, 0);

                // Get the AudioSource component and play clip
                audioSource = GetComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.Play();

                //Start the coroutine we define below named ExampleCoroutine.
                StartCoroutine(WaitTillFinished());

                Debug.Log("Speech synthesis success!");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Debug.Log($"CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]");
            }
        }
    }

    IEnumerator WaitTillFinished()
    {
        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(2);

        // Start listening again on sucess.
        speech.ButtonClick();
    }
}
