using System.Collections;
using System.Collections.Generic;
using FMODPlus;
using Michsky.DreamOS;
using NUnit.Framework.Constraints;
using Sophia.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainVideoController : MonoBehaviour
{
    public enum E_VIDEO_NAME { None, Main }
    E_VIDEO_NAME currentVideo = E_VIDEO_NAME.None;
    [SerializeField] GameObject[] videoList;
    VideoPlayer vid;
    RawImage image;
    public RawImage moviePanel;
    [SerializeField] FMODAudioSource fMODAudioSource;
    [SerializeField] CommandSender commandStarter;
    [SerializeField] CommandSender commandEnder;
    [SerializeField] private bool IsVideoRun;
    private float movieTime = 0;
    public void StartVideo(E_VIDEO_NAME video)
    {
        if (!IsVideoRun)
        {
            IsVideoRun = true;
            image = videoList[(int)video].transform.GetChild(0).GetComponent<RawImage>();
            vid = videoList[(int)video].transform.GetChild(1).GetComponent<VideoPlayer>();
            currentVideo = video;
            fMODAudioSource = videoList[(int)video].transform.GetChild(1).GetComponent<FMODAudioSource>();
            StartCoroutine(PanelFadeIn(moviePanel));
            vid.loopPointReached += VideoEnd;
        }

    }

    public void PauseVideo()
    {
        vid.Pause();
        fMODAudioSource.Pause();
    }

    public void PlayVideo()
    {
        image.enabled = true;
        fMODAudioSource?.Play();
        vid.Play();
    }

    void Update()
    {
        if (!IsVideoRun)
        {
            movieTime += Time.deltaTime;
            if ((int)movieTime >= 5)
            {
                StartVideo(E_VIDEO_NAME.Main);
            }
        }
    }

    IEnumerator PanelFadeIn(RawImage panel)
    {
        panel.enabled = true;
        Color fadeColor = panel.color;
        fadeColor.a = 0;

        while (fadeColor.a < 1f)
        {
            fadeColor.a += 0.05f;
            panel.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.07f);
        }
        commandEnder.SendCommand();
        PlayVideo();
        yield return new WaitForSecondsRealtime(0.1f);
        panel.enabled = false;
    }

    void VideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        switch (currentVideo)
        {
            case E_VIDEO_NAME.Main:
                IsVideoRun = false;
                movieTime = 0;
                commandStarter.SendCommand();
                break;
        }
        image.enabled = false;
        vid.Stop();
        fMODAudioSource.Stop();
        IsVideoRun = false;
    }

}
