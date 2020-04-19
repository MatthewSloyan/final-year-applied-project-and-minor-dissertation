using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class TextToSpeech : MonoBehaviour
{
    #region == Private Variables == 
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private int voiceOption;

    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;

    private object threadLocker = new object();
    private bool waitingForSpeak;
    private bool ifChecked;
    //private string message;

    private SpeechToText speech;

    private static Collider collision = null;
    public static Collider Collision
    {
        get { return collision; }
        set { collision = value; }
    }
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

    public void ConvertTextToSpeech(string inputText, string voiceName, bool ifChecked)
    {
        this.ifChecked = ifChecked;

        // Creates an instance of a speech config with specified subscription key and service region.
        // For security this is read in from a text file and is not included on Github. 
        // API_Key.txt is stored in the root directory of the project
        // For now just using free trial key.
        //string API_Key = System.IO.File.ReadAllText("../../API_Key.txt");

        speechConfig = SpeechConfig.FromSubscription("a2f75e30125b4f099012eaa7f9a5c840", "westeurope");
        //speechConfig = SpeechConfig.FromSubscription("722faee502a24ebeb53cf34a58e22e7d", "westus");

        // The default format is Riff16Khz16BitMonoPcm.
        // We are playing the audio in memory as audio clip, which doesn't require riff header.
        // So we need to set the format to Raw16Khz16BitMonoPcm.
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw16Khz16BitMonoPcm);

        // Change voice depending on option selected for multiple characters of different genders and ethnicities.
        speechConfig.SpeechSynthesisVoiceName = voiceName;

        lock (threadLocker)
        {
            waitingForSpeak = true;
        }

        // Creates a speech synthesizer.
        synthesizer = new SpeechSynthesizer(speechConfig, null);

        // Starts speech synthesis, and returns after a single utterance is synthesized.
        // There was an issue where the game would freeze at this point "synthesizer.SpeakTextAsync(inputText)" blocks until the task is complete. 
        // To solve this I have implemeted a task which runs using a Coroutine that waits until the result from Azure is returned. 
        // Code is adapted from: https://stackoverflow.com/questions/57897464/unity-freezes-for-2-seconds-while-microsoft-azure-text-to-speech-processes-input
        Task<SpeechSynthesisResult> task = synthesizer.SpeakTextAsync(inputText);

        StartCoroutine(RunSynthesizer(task, speechConfig, synthesizer));

        lock (threadLocker)
        {
            waitingForSpeak = false;
        }
    }

    private IEnumerator RunSynthesizer(Task<SpeechSynthesisResult> task, SpeechConfig config, SpeechSynthesizer synthesizer)
    {
        // Wait until the task is complete E.g Azure Text to Speech returns a result.
        yield return new WaitUntil(() => task.IsCompleted);

        var result = task.Result;
        
        // Check result.
        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            // Create a task to create audio data and wait till it's completed.
            Task<float[]> audioTask = getAudioData(result);
            yield return new WaitUntil(() => audioTask.IsCompleted);

            // Synthesize the Audio and play to the speaker.
            // The output audio format is 16K 16bit mono
            var sampleCount = result.AudioData.Length / 2;
            var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
            audioClip.SetData(audioTask.Result, 0);
            audioSource.clip = audioClip;
            audioSource.Play();
            
            // true if ticket has already been checked, otherwise start listening again until ticket has been checked.
            if (!ifChecked)
            {
                // Turn NPC's head when speaking.
                try
                {
                    if (Math.Round(collision.gameObject.transform.rotation.eulerAngles.y, 0) == 90){
                        collision.gameObject.GetComponent<Animator>().SetTrigger("TurnHeadLeft");
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Animator>().SetTrigger("TurnHeadRight");
                    }
                }
                catch{}

                // Wait until the audio is completed and start listening again.
                // Code adapted from: https://answers.unity.com/questions/1111236/wait-for-audio-to-finish-and-then-load-scene.html
                yield return new WaitWhile(() => audioSource.isPlaying);
                speech.convertSpeechToText(speech.GetSessionID(), speech.GetPersona(), speech.GetVoiceName(),speech.GetHasTicket());
            }
        }

        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            Debug.Log($"CANCELED:\nReason=[{cancellation.Reason}]\nErrorDetails=[{cancellation.ErrorDetails}]");
        }

        // Dispose of synthesizer correctly.
        synthesizer.Dispose();
    }

    // Task method which gets 16K 16bit mono audio from the result to output to the speaker.
    // Code is adapted from: https://stackoverflow.com/questions/57897464/unity-freezes-for-2-seconds-while-microsoft-azure-text-to-speech-processes-input
    private Task<float[]> getAudioData(SpeechSynthesisResult result)
    {
        var sampleCount = result.AudioData.Length / 2;
        var audioData = new float[sampleCount];

        for (var i = 0; i < sampleCount; ++i) {
            audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
        }

        return Task.FromResult(audioData);
    }
}
