using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
//Handles Playing the presentation video
public class PlayVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    void OnTriggerEnter(Collider col)
    {
        // Start playing the video, when player walks into colider.
        StartCoroutine(StartVideo());
    }

    private void OnTriggerExit(Collider col)
    {
        // Stop the playing video.
        StopVideo();
    }

    // Code adapted from: https://www.mirimad.com/unity-play-video-on-canvas/
    IEnumerator StartVideo(){
        // Stop video if playing.
        //StopVideo();

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

    void StopVideo(){
        // Stop video and audio.
        videoPlayer.Stop();
        audioSource.Stop();
    }
}
