using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    private UnityAction onCompleteCallback;

    // Start is called before the first frame update

    public FadeUI AddBindingAction(UnityAction unityAction)
    {
        onCompleteCallback = null;
        onCompleteCallback = unityAction;
        return this;
    }

    public void FadeIn(float fadeTime, float fadeDuration)
    {
        StartCoroutine(CoFadeIn(fadeTime, fadeDuration));
    }

    public void FadeOut(float fadeTime, float fadeDuration)
    {
        StartCoroutine(CoFadeOut(fadeTime, fadeDuration));
    }

    public void FadePanelOn()
    {
        fadePanel.gameObject.SetActive(true);
    }
    public void FadePanelOff()
    {
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator CoFadeIn(float fadeTime, float fadeDuration)
    {
        fadePanel.gameObject.SetActive(true);
        float passedTime = 0;

        while(passedTime < fadeDuration) {
            Color c = fadePanel.color;
            c.a = 1f - (passedTime / 1f);
            fadePanel.color = c;
            passedTime += fadeTime;
            yield return new WaitForSecondsRealtime(fadeTime);
        }
        
        if(onCompleteCallback != null)
            onCompleteCallback.Invoke();
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator CoFadeOut(float fadeTime, float fadeDuration)
    {
        fadePanel.gameObject.SetActive(true);
        float passedTime = 0;
        
        while(passedTime < fadeDuration) {
            Color c = fadePanel.color;
            c.a = passedTime / 1f;
            fadePanel.color = c;
            passedTime += fadeTime;
            yield return new WaitForSecondsRealtime(fadeTime);
        }

        if(onCompleteCallback != null)
            onCompleteCallback.Invoke();

        if(!StoryManager.Instance.IsTutorial){
            fadePanel.gameObject.SetActive(false);
            FadeIn(0.01f,3.0f);
        }
    }
}
