using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManger : MonoBehaviour
{
    public VideoPlayer video;
    public RenderTexture renderTexture;
    // Start is called before the first frame update
    private void Awake()
    {
        renderTexture.Release();
        video.Prepare();
    }
    private void Start()
    {
        video.loopPointReached += CheckOver;
        video.SetDirectAudioVolume(0,1);
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("Video Is Over");
        StoryManager.Instance.IsTutorial = false;
        SceneManager.LoadScene("_01_Chapter_kabocha");
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){
            Debug.Log("멈춤신호 감지!");
            StoryManager.Instance.IsTutorial = false;
            SceneManager.LoadScene("_01_Chapter_kabocha");
        }
    }

}