using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoryBar : MonoBehaviour
{
    [SerializeField] public Image bigDarkBar;
    [SerializeField] public Image smallDarkBar;
    [SerializeField] public Image speaker;
    [SerializeField] public Image textBar;
    [SerializeField] public Image textCursor;
    [SerializeField] public RawImage storyFadePanel;

    public bool IsWaitOver = false;
    public void FadeIn(Image image)
    {
        StartCoroutine(CoFadeIn(image));
    }

    public void FadeOut(float fadeTime, float fadeDuration)
    {
        StartCoroutine(CoFadeOut(fadeTime, fadeDuration));
    }
    public void WaitAfterBoss()
    {
        StartCoroutine(CoWaitAfterBoss());
    }

    public void SetTransparent()
    {
        MakeTransparent(bigDarkBar);
        MakeTransparent(smallDarkBar);
        MakeTransparent(speaker);
        MakeTransparent(textBar);
        MakeTransparent(textCursor);
    }
    public void MakeTransparent(Image image)
    {
        Color transparent = image.color;
        transparent.a = 0;
        image.color = transparent;
    }
    public void fadeStoryBarOn()
    {
        FadeIn(bigDarkBar);
        FadeIn(smallDarkBar);
        FadeIn(speaker);
        FadeIn(textBar);
        FadeIn(textCursor);
    }

    IEnumerator CoFadeIn(Image image)
    {
        image.gameObject.SetActive(true);
        Color fadeColor = image.color;
        fadeColor.a = 0;

        while(fadeColor.a < 1f) {
            fadeColor.a += 0.05f;
            image.color = fadeColor;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
        //image.gameObject.SetActive(false);
    }
    IEnumerator CoFadeOut(float fadeTime, float fadeDuration)
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
        TextManager.Instance.SetDialogue();
        TextManager.Instance.talkPanel.SetActive(true);
        fadeStoryBarOn();
        IsWaitOver = true;
    }
    IEnumerator CoWaitAfterBoss()
    {
        int second = 0;
        while(second <= 2)
        {
            second++;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        FadeOut(0.05f,2f);
    }
}
