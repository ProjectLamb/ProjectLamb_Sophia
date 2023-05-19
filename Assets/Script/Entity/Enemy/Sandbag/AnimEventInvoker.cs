using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public enum Enum_AnimState {
    Idle, Attack, Jump, Die
}

public class AnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void IdleStart(){animCallback[(int)Enum_AnimState.Idle].Invoke();}
    //public void IdleEnd(){animCallback[(int)Enum_AnimState.Idle]?.Invoke();}
    public void AttackStart(){animCallback[(int)Enum_AnimState.Attack].Invoke();}
    //public void AttackEnd(){animCallback[(int)Enum_AnimState.Attack]?.Invoke();}
    public void JumpStart(){animCallback[(int)Enum_AnimState.Jump].Invoke();}
    //public void JumpEnd(){animCallback[(int)Enum_AnimState.Jump]?.Invoke();}
    public void DieStart(){animCallback[(int)Enum_AnimState.Die].Invoke();}
}
