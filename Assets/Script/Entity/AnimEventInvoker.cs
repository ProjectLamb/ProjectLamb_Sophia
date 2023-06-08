using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class AnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void IdleStart(){animCallback[(int)E_AnimState.Idle].Invoke();}
    //public void IdleEnd(){animCallback[(int)E_AnimState.Idle]?.Invoke();}
    public void AttackStart(){animCallback[(int)E_AnimState.Attack].Invoke();}
    //public void AttackEnd(){animCallback[(int)E_AnimState.Attack]?.Invoke();}
    public void JumpStart(){animCallback[(int)E_AnimState.Jump].Invoke();}
    //public void JumpEnd(){animCallback[(int)E_AnimState.Jump]?.Invoke();}
    public void DieStart(){animCallback[(int)E_AnimState.Die].Invoke();}
}
