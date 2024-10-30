using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoryBarFade : MonoBehaviour
{
    [SerializeField] public CanvasGroup storyBar;
    [SerializeField] public RawImage storyFadePanel;

    public bool IsWaitOver = false;

    // 검은색 패널 fadeout
    public void PanelFadeOut(float fadeTime, float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeTime, fadeDuration));
    }
    //스토리바 페이드인
    public void FadeStoryBarIn()
    {
        StartCoroutine(StoryFadeIn(storyBar));
    }

    IEnumerator StoryFadeIn(CanvasGroup cg)
    {
        cg.alpha = 0;
        while(cg.alpha < 1f) {
            cg.alpha += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    IEnumerator FadeOut(float fadeTime, float fadeDuration)
    {
        storyFadePanel.gameObject.SetActive(true);
        float passedTime = 0;
        
        while(passedTime < fadeDuration) {
            Color c = storyFadePanel.color;
            c.a = passedTime / 1f;
            storyFadePanel.color = c;
            passedTime += fadeTime;
            yield return new WaitForSecondsRealtime(fadeTime);
        }
        TextManager.Instance.storyEventName = "AfterBoss";
        TextManager.Instance.IsStory = true;
        //TextManager.Instance.IsOnce = false;
        TextManager.Instance.IsBlockTextUpdate = false;
        TextManager.Instance.SetDialogue();
        TextManager.Instance.talkPanel.SetActive(true);
        FadeStoryBarIn();
        IsWaitOver = true;
    }

    // 보스 처치 이후
    // IEnumerator WaitAfterBoss()
    // {
    //     int second = 0;
    //     while(second <= 2)
    //     {
    //         second++;
    //         yield return new WaitForSecondsRealtime(1.0f);
    //     }
    //     FadeOut(0.05f,2f);
    // }
}
