using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PileShuffleController : MonoBehaviour
{
    [SerializeField] VideoPlayer m_videoPlayer;
    public double VideoLength { get; private set; }
    public void Init()
    {
        gameObject.SetActive(false);
        // Prepare the video to be ready to play from the start
        m_videoPlayer.Stop();
        //m_videoPlayer.frame = 0;
        VideoLength = m_videoPlayer.length;
        VideoLength = 0;
        // Register to the loopPointReached event
        m_videoPlayer.loopPointReached += OnVideoEnd;
    }


    // This method will be called when the video reaches its end
    void OnVideoEnd(VideoPlayer vp)
    {
        gameObject.SetActive(false);
        m_videoPlayer.Stop();
        m_videoPlayer.frame = 0;
    }

    public void PlayVideo()
    {
        if (VideoLength == 0)
            return;
        gameObject.SetActive(true);
        m_videoPlayer.frame = 0;
        m_videoPlayer.Prepare();
        m_videoPlayer.Play();
    }
}
