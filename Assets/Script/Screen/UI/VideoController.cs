using System.Collections;
using System.Collections.Generic;
using FMODPlus;
using Michsky.DreamOS;
using NUnit.Framework.Constraints;
using Sophia.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public enum E_VIDEO_NAME { None, Opening, ElderOne, }
    E_VIDEO_NAME currentVideo = E_VIDEO_NAME.None;
    [SerializeField] GameObject[] videoList;
    VideoPlayer vid;
    RawImage image;
    [SerializeField] FMODAudioSource fMODAudioSource;
    [SerializeField] CommandSender commandStarter;
    [SerializeField] CommandSender commandEnder;
    [SerializeField] CommandSender bossStateStarter;
    [SerializeField] private bool isSkippable;
    [SerializeField] public Canvas skipCanvas;
    [SerializeField] public Image skipBar;
    public void StartVideo(E_VIDEO_NAME video)
    {
        isSkippable = false; //스킵 가능 여부
        image = videoList[(int)video].transform.GetChild(0).GetComponent<RawImage>();
        vid = videoList[(int)video].transform.GetChild(1).GetComponent<VideoPlayer>();
        currentVideo = video;
        fMODAudioSource = videoList[(int)video].transform.GetChild(1).GetComponent<FMODAudioSource>();
        vid.loopPointReached += VideoEnd;
        PauseMenu.OnOpenMenuStaticEvent.AddListener(PauseVideo);
        PauseMenu.OnCloseMenuStaticEvent.AddListener(PlayVideo);
        StartCoroutine(CoFadeIn(0.02f, 1f));
        skipCanvas.enabled = true;
    }

    public void PauseVideo() {
        vid.Pause();
        fMODAudioSource.Pause();
    }

    public void PlayVideo() {
        vid.Play();
        fMODAudioSource.UnPause();
    }

    void Update()
    {
        if (!isSkippable)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                skipBar.fillAmount += 0.03f;
                //스킵 하시겠습니까? UI 띄우기
                //VideoEnd(vid);
                //isSkippable = false;
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                skipBar.fillAmount = 0;
            }
            if(skipBar.fillAmount >= 1)
            {
                VideoEnd(vid);
                isSkippable = false;
                skipCanvas.enabled = false;
            }
        }
    }

    IEnumerator CoFadeIn(float fadeTime, float fadeDuration)
    {
        GameManager.Instance.GlobalEvent.Pause(gameObject.name);
        yield return new WaitForEndOfFrame();
        image.color = Color.black;
        image.enabled = true;
        commandEnder.SendCommand();
        vid.Play();
        fMODAudioSource?.Play();
        float passedTime = 0;
        while (passedTime < fadeDuration)
        {
            Color c = image.color;
            c = (new Vector4(1, 1, 1, 0) * (passedTime / fadeDuration)) + new Vector4(0, 0, 0, 1);
            image.color = c;
            passedTime += fadeTime;
            yield return new WaitForSecondsRealtime(fadeTime);
        }
        InGameScreenUI.Instance._fadeUI.FadePanelOff();
    }

    void VideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        InGameScreenUI.Instance._fadeUI.AddBindingAction(() =>
        {
            GameManager.Instance.GlobalEvent.Play(gameObject.name);;

            switch (currentVideo)
            {
                case E_VIDEO_NAME.ElderOne:
                    InGameScreenUI.Instance._bossHealthBar.SetActive(true);
                    InGameScreenUI.Instance._fadeUI.FadePanelOff();
                    commandStarter.SendCommand();
                    bossStateStarter.SendCommand();
                    break;
                case E_VIDEO_NAME.Opening :
                    StoryManager.Instance.IsTutorial = false;
                    InGameScreenUI.Instance._fadeUI.FadePanelOff();
                    commandStarter.SendCommand();
                    
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.IsTutorial = false;
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.CutSceneSaveData.IsSkipStory = true;
                    DontDestroyGameManager.Instance.SaveLoadManager.SaveAsJson();
                    break;
            }
            image.enabled = false;
            vid.Stop();
            fMODAudioSource.Stop();
            PauseMenu.OnOpenMenuStaticEvent.RemoveListener(PauseVideo);
            PauseMenu.OnCloseMenuStaticEvent.RemoveListener(PlayVideo);
        });
        InGameScreenUI.Instance._fadeUI.FadeOut(0.02f, 1.5f);
    }
}
