using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using FMODPlus;
using UnityEngine;
using UnityEngine.UI;
using AudioType = FMODPlus.AudioType;

public class AudioSetterUIComponent : MonoBehaviour{

    [SerializeField] private Slider _audioSlider;
    public Slider AudioSlider {get {return _audioSlider;}}
}