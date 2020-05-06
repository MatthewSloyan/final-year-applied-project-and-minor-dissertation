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

    public AudioClip[] audioClips;
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
        PlayAudio(0);
        Debug.Log("AUDIO PLAYING");
    }

    //Turn the sound off
    public void turnSoundOnOff(int option)
    {
        if (option == 1)
        {
            sourceMusic.mute = false;
        }
        else
        {
            sourceMusic.mute = true;
        }
    }

    // Play background music from Dictionary.
    public void PlayAudio(int clipIndex)
    {
        sourceMusic.clip = audioClips[clipIndex];
        sourceMusic.Play();
    }
    
    // Play audio just once over background music.
    public void PlayAudioOnce(int clipIndex)
    {
        sourceMusic.PlayOneShot(audioClips[clipIndex]);
    }

}
