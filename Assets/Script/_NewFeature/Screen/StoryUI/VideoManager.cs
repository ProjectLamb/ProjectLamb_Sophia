using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManger : MonoBehaviour
{
    public VideoPlayer video;
    public RenderTexture renderTexture;
    // Start is called before the first frame update
    private void Awake()
    {
        renderTexture.Release();
    }
    private void Update()
    {
        
    }

}