using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.EventInstance SFXVolumeTestEvent;

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus Master;
    float MusicVolume = 0.5f;
    float SFXVolume = 0.5f;
    float MasterVolume = 1f;

    private void Awake() {
        Music = FMODUnity.RuntimeManager.GetBus("");
        SFX = FMODUnity.RuntimeManager.GetBus("");
        Master = FMODUnity.RuntimeManager.GetBus("");
        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("");
    }

    private void Update() {
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
        //Music.setPaused();
    }

    public void SFXVolumeLevel(float newSFXVolume){
        SFXVolume = newSFXVolume;
        FMOD.Studio.PLAYBACK_STATE PbStage;
        SFXVolumeTestEvent.getPlaybackState(out PbStage);
        if(PbStage != FMOD.Studio.PLAYBACK_STATE.PLAYING){
            SFXVolumeTestEvent.start();
        }
    }
}
