using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// Class which handles Audio for in game
public class AudioController : MonoBehaviour
{
    #region == Private Variables == 

    [SerializeField]
    private AudioSource sourceMusic;

    // Dictionary which holds all audio files in memory.
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    #endregion

    // Singleton design pattern to get instance of class
    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Load in all clips into memory, and start background music.
        LoadAllAudioClips();
        PlayAudio("train-station-ambience-1");
        Debug.Log("AUDIO PLAYING");
    }

    void Update()
    {
        // Check if game audio is off using player prefs, if so mute audiosource.
        //PlayerPrefs.DeleteAll();
        // if (PlayerPrefs.GetString("Sound") == "True")
        // {
        //     sourceMusic.mute = false;
        // }
        // else
        // {
        //     sourceMusic.mute = true;
        // }
    }

    // Play background music from Dictionary.
    public void PlayAudio(string fileName)
    {
        sourceMusic.clip = audioClips[fileName];
        sourceMusic.Play();
    }
    
    // Play audio just once over background music.
    public void PlayAudioOnce(string fileName)
    {
        sourceMusic.PlayOneShot(audioClips[fileName]);
    }

    private void LoadAllAudioClips()
    {
        // Get all file names from Resources folder, and get file information about each.
        // Code adapted from: https://answers.unity.com/questions/16433/get-list-of-all-files-in-a-directory.html
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Audio");
        FileInfo[] fileInfo = dir.GetFiles();

        // Loop through each file in the directory/array.
        foreach (FileInfo file in fileInfo)
        {
            // Exlude meta files
            if (!file.Name.Contains(".meta"))
            {
                // Remove file extension from file name, get file from resouces and add to dictionary.
                string[] fileName = file.Name.Split('.');
                
                AudioClip temp = Resources.Load("Audio/" + fileName[0]) as AudioClip;
                audioClips.Add(fileName[0], temp);
            }
        }
    }
}
