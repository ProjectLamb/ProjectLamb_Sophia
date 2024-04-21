using System.Collections;
using System.Collections.Generic;
using FMODPlus;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public enum E_UIAUDIO_INDEX {Click, GameStart, Hovering, ClickAccept, Failure, OpenESC, CloseESC};
    [SerializeField] FMODAudioSource[] _audioSource;
    public void PlayClickSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.Click].Play();
    }

    public void PlayGameStartSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.GameStart].Play();
    }

    public void PlayHoveringSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.Hovering].Play();
    }
    public void PlayClickAcceptSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.ClickAccept].Play();
    }

    public void PlayFailureSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.Failure].Play();
    }

    public void PlayOpenESCSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.OpenESC].Play();
    }
    public void PlayCloseESCSound()
    {
        _audioSource[(int)E_UIAUDIO_INDEX.CloseESC].Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
