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
    [SerializeField] Image[] manualList;
    VideoPlayer vid;
    RawImage image;
    [SerializeField] FMODAudioSource fMODAudioSource;
    [SerializeField] CommandSender commandStarter;
    [SerializeField] CommandSender commandEnder;
    [SerializeField] CommandSender bossStateStarter;
    [SerializeField] private bool isSkippable;
    [SerializeField] private bool isVideoStart;
    [SerializeField] private bool isManualFadeOn;
    [SerializeField] private bool isManualFadeOff;
    [SerializeField] private int manualIndex;
    [SerializeField] public Canvas skipCanvas;
    [SerializeField] public Image skipBar;
    public void StartVideo(E_VIDEO_NAME video)
    {
        isVideoStart = true;
        isSkippable = false; //스킵 가능 여부
        isManualFadeOn = false;
        isManualFadeOff = true;
        manualIndex = -1;
        image = videoList[(int)video].transform.GetChild(0).GetComponent<RawImage>();
        vid = videoList[(int)video].transform.GetChild(1).GetComponent<VideoPlayer>();
        currentVideo = video;
        fMODAudioSource = videoList[(int)video].transform.GetChild(1).GetComponent<FMODAudioSource>();
        vid.loopPointReached += VideoEnd;
        PauseMenu.OnOpenMenuStaticEvent.AddListener(PauseVideo);
        PauseMenu.OnCloseMenuStaticEvent.AddListener(PlayVideo);
        StartCoroutine(CoFadeIn(0.02f, 1f));
    }

    public void PauseVideo()
    {
        vid.Pause();
        fMODAudioSource.Pause();
    }

    public void PlayVideo()
    {
        vid.Play();
        fMODAudioSource.UnPause();
    }

    void Update()
    {
        if (!isSkippable)
        {
            if (Input.GetKey(KeyCode.Space) && isVideoStart)
            {
                skipCanvas.enabled = true;
                skipBar.fillAmount += 0.03f;
            }
            else if (Input.GetKeyUp(KeyCode.Space)) // 스페이스바를 뗐을때
            {
                skipBar.fillAmount = 0;
                if (skipBar.fillAmount == 0) skipCanvas.enabled = false;
            }
            if (skipBar.fillAmount >= 1) // 스킵버튼이 꾹 눌러지면
            {
                VideoEnd(vid);
                isVideoStart = false;
                isSkippable = false;
                skipCanvas.enabled = false;
            }
            if (Input.GetKey(KeyCode.Space) && isManualFadeOn && !isManualFadeOff) // 조작법 UI가 다 뜬 이후에 스페이스가 입력되었을 시
            {
                isManualFadeOn = false;
                isManualFadeOff = true;
                StartCoroutine(imgFadeOut(manualList[manualIndex - 1]));
            }
            else if (!isVideoStart) skipCanvas.enabled = false;
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
            GameManager.Instance.GlobalEvent.Play(gameObject.name); ;

            switch (currentVideo)
            {
                case E_VIDEO_NAME.ElderOne:
                    InGameScreenUI.Instance._bossHealthBar.SetActive(true);
                    InGameScreenUI.Instance._fadeUI.FadePanelOff();
                    isVideoStart = false;
                    skipCanvas.enabled = false;
                    commandStarter.SendCommand();
                    bossStateStarter.SendCommand();
                    break; 
                case E_VIDEO_NAME.Opening:
                    if (!isManualFadeOn && isManualFadeOff && manualIndex < 0) // 비디오가 끝나고 단 한번만 조작법 페이드
                    {
                        StartCoroutine(imgFadeIn(manualList[0]));
                        manualIndex = 0;
                    }
                    isVideoStart = false;
                    skipCanvas.enabled = false;
                    InGameScreenUI.Instance._fadeUI.FadePanelOff();
                    commandStarter.SendCommand();
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.IsNewFile = false;
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
    IEnumerator imgFadeIn(Image image)
    {
        image.gameObject.SetActive(true);
        Color fadeColor = image.color;
        fadeColor.a = 0;

        while (fadeColor.a < 1f)
        {
            fadeColor.a += 0.03f;
            image.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(1.0f);
        isManualFadeOn = true;
        isManualFadeOff = false;
        manualIndex++;
        StopCoroutine(imgFadeIn(image));
    }
    IEnumerator imgFadeOut(Image image)
    {
        Color fadeColor = image.color;
        fadeColor.a = 1;

        while (fadeColor.a > 0f)
        {
            fadeColor.a -= 0.03f;
            image.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        image.gameObject.SetActive(false);
        if (!isManualFadeOn && isManualFadeOff && manualIndex < manualList.Length)
        {
            StartCoroutine(imgFadeIn(manualList[manualIndex]));
        }
        if (manualIndex >= manualList.Length)
        {
            StoryManager.Instance.IsTutorial = false; // 튜토리얼 종료 판정
        }
        StopCoroutine(imgFadeOut(image));
    }

}
