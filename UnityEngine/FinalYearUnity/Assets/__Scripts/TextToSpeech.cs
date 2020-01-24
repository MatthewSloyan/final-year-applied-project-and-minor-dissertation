using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    #region == Private Variables == 
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private int voiceOption;

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
        speech = SpeechToText.Instance;
    }

    public void ConvertTextToSpeech(string inputText)
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // For security this is read in from a text file and is not included on Github. 
        // API_Key.txt is stored in the root directory of the project
        // For now just using free trial key.
        string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

        speechConfig = SpeechConfig.FromSubscription(API_Key, "westeurope");
        //speechConfig = SpeechConfig.FromSubscription("722faee502a24ebeb53cf34a58e22e7d", "westus");

        // The default format is Riff16Khz16BitMonoPcm.
        // We are playing the audio in memory as audio clip, which doesn't require riff header.
        // So we need to set the format to Raw16Khz16BitMonoPcm.
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);

        // Change voice depending on option selected for multiple characters of different genders and ethnicities.
        switch (voiceOption)
        {
            case 1:
                speechConfig.SpeechSynthesisVoiceName = "en-US-JessaNeural";
                break;
            case 2:
                speechConfig.SpeechSynthesisVoiceName = "en-US-GuyNeural";
                break;
            case 3:
                speechConfig.SpeechSynthesisVoiceName = "en-IE-Sean";
                break;
            case 4:
                speechConfig.SpeechSynthesisVoiceName = "de-DE-KatjaNeural";
                break;
            default:
                speechConfig.SpeechSynthesisVoiceName = "en-US-JessaNeural";
                break;
        }

        // Creates a speech synthesizer.
        synthesizer = new SpeechSynthesizer(speechConfig, null);

        // Starts speech synthesis, and returns after a single utterance is synthesized.
        using (var result = synthesizer.SpeakTextAsync(inputText).Result)
        {
            // Checks result.
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                // Native playback is not supported on Unity yet (currently only supported on Windows/Linux Desktop).
                // Use the Unity API to play audio here as a short term solution.
                // Native playback support will be added in the future release.
                var sampleCount = result.AudioData.Length / 2;
                var audioData = new float[sampleCount];
                for (var i = 0; i < sampleCount; ++i)
                {
                    audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
                }

                // The output audio format is 16K 16bit mono
                var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
                audioClip.SetData(audioData, 0);
                audioSource.clip = audioClip;
                audioSource.Play();
                
                Debug.Log("Speech synthesis success!");

                //Start the coroutine we define below named WaitTillFinished.
                // Code adapted from: https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
                StartCoroutine(WaitTillFinished());
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
        yield return new WaitForSeconds(2.5f);

        // Start listening for speech again on sucess.
        speech.convertSpeechToText(speech.GetSessionID());
    }

    void OnDestroy()
    {
        synthesizer.Dispose();
    }
}
