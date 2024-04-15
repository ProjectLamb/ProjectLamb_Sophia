using System.Collections;
using System.Collections.Generic;
using FMODPlus;
using Michsky.DreamOS;
using Sophia.UserInterface;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] VideoPlayer vid;
    [SerializeField] RawImage image;
    [SerializeField] FMODAudioSource fMODAudioSource;
    [SerializeField] CommandSender commandStarter;
    [SerializeField] CommandSender commandEnder;
    // Start is called before the first frame update
    void Start()
    {
        vid.loopPointReached += VideoEnd;
        image.enabled = false;
    }
    public void StartVideo()
    {
        StartCoroutine(CoFadeIn(0.02f, 0.1f));
    }

    IEnumerator CoFadeIn(float fadeTime, float fadeDuration)
    {
        GameManager.Instance.GlobalEvent.IsGamePaused = true;
        yield return new WaitForEndOfFrame();

        image.color = Color.black;
        image.enabled = true;
        commandEnder.SendCommand();
        vid.Play();
        fMODAudioSource?.Play();
        
        float passedTime = 0;
        while(passedTime < fadeDuration) {
            Color c = image.color;
            c = (new Vector4(1,1,1,0) * (passedTime / fadeDuration)) + new Vector4(0,0,0,1);
            image.color = c;
            passedTime += fadeTime;
            yield return new WaitForSecondsRealtime(fadeTime);
        }        
    }

    void VideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("Video End");

        InGameScreenUI.Instance._fadeUI.AddBindingAction(() =>
        {
            GameManager.Instance.GlobalEvent.IsGamePaused = false;
            InGameScreenUI.Instance._bossHealthBar.SetActive(true);
            commandStarter.SendCommand();
            image.enabled = false;
            vid.Stop();
        });

        InGameScreenUI.Instance._fadeUI.FadeOut(0.02f, 0.5f);
    }
}
