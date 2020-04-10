using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SplashSceneController : MonoBehaviour
{
    // Source: https://mirimad.com/unity-play-video-on-canvas/
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    // Use this for initialization
    void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        StartCoroutine(PlayVideo());
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
        }
    }

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;
        rawImage.texture = videoPlayer.texture;
        rawImage.color = Color.white;
        videoPlayer.Play();
    }

}
