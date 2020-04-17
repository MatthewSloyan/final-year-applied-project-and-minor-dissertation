using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    
    // Use this for initialization
    void Start () {
        // Start playing the video.
        StartCoroutine(StartVideo());
    }

    // Code adapted from: https://www.mirimad.com/unity-play-video-on-canvas/
    IEnumerator StartVideo(){
        // Prepare video and wait till ready.
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
        }

        // Once video is ready, load into canvas and play video and audio.
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
}
