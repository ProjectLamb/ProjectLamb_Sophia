
using System;
using FMODPlus;
using UnityEngine;
using AudioType = FMODPlus.AudioType;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-221)]
public class CommandSenderLink : MonoBehaviour
{
    [SerializeField] private CommandSender[] _senders;

    [Tooltip("Audio Manager에 있는 오디오 타입을 선택합니다.")]
    public AudioType AudioType = AudioType.BGM;

    private void Awake()
    {
        // Init()
    }

    public void Init()
    {
            _senders = GetComponentsInChildren<CommandSender>();

            foreach (CommandSender sender in _senders)
            {
                switch (AudioType)
                {
                    case AudioType.AMB: {
                        sender.audioSource = DontDestroyGameManager.Instance.AudioManager.AMBAudioSource;
                        break;
                    }
                    case AudioType.BGM: {
                        sender.audioSource = DontDestroyGameManager.Instance.AudioManager.BGMAudioSource;
                        break;
                    }
                    case AudioType.SFX: {
                        sender.audioSource = DontDestroyGameManager.Instance.AudioManager.SFXAudioSource;
                        break;
                    }
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
                    DontDestroyGameManager.Instance.AudioManager.AMBAudioSource.KeyOff();
                    break;
                case AudioType.BGM:
                    DontDestroyGameManager.Instance.AudioManager.BGMAudioSource.KeyOff();
                    break;
                case AudioType.SFX:
                    DontDestroyGameManager.Instance.AudioManager.SFXAudioSource.KeyOff();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}