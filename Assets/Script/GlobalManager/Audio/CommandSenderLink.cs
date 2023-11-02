
using System;
using FMODPlus;
using UnityEngine;
using AudioType = FMODPlus.AudioType;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-221)]
public class CommandSenderLink : MonoBehaviour
{
    private CommandSender[] _senders;

    [Tooltip("Audio Manager에 있는 오디오 타입을 선택합니다.")]
    public AudioType AudioType = AudioType.BGM;

    private void Awake()
    {
        _senders = GetComponentsInChildren<CommandSender>();

        foreach (CommandSender sender in _senders)
        {
            switch (AudioType)
            {
                case AudioType.AMB:
                    sender.audioSource = GameManager.Instance.GlobalAudioManager.AMBAudioSource;
                    break;
                case AudioType.BGM:
                    sender.audioSource = GameManager.Instance.GlobalAudioManager.BGMAudioSource;
                    break;
                case AudioType.SFX:
                    sender.audioSource = GameManager.Instance.GlobalAudioManager.SFXAudioSource;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void KeyOff()
    {
        foreach (CommandSender sender in _senders)
        {
            switch (AudioType)
            {
                case AudioType.AMB:
                    GameManager.Instance.GlobalAudioManager.AMBAudioSource.KeyOff();
                    break;
                case AudioType.BGM:
                    GameManager.Instance.GlobalAudioManager.BGMAudioSource.KeyOff();
                    break;
                case AudioType.SFX:
                    GameManager.Instance.GlobalAudioManager.SFXAudioSource.KeyOff();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}