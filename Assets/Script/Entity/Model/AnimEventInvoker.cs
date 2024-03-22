using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public enum ANIME_STATE
{
    IDLE, ATTACK, JUMP, DIE
}

public class AnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void IdleStart(){animCallback[(int)ANIME_STATE.IDLE].Invoke();}
    //public void IdleEnd(){animCallback[(int)ANIME_STATE.Idle]?.Invoke();}
    public void AttackStart(){animCallback[(int)ANIME_STATE.ATTACK].Invoke();}
    //public void AttackEnd(){animCallback[(int)ANIME_STATE.Attack]?.Invoke();}
    public void JumpStart(){animCallback[(int)ANIME_STATE.JUMP].Invoke();}
    //public void JumpEnd(){animCallback[(int)ANIME_STATE.Jump]?.Invoke();}
    public void DieStart(){animCallback[(int)ANIME_STATE.DIE].Invoke();}
}