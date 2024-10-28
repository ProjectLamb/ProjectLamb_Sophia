using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ELDERONE_ANIME_STATE
{
    Swing, WaveAttack, Dirt, AttackReady, SpecialReady,
}

public class ElderOneAnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void Swing(){animCallback[(int)ELDERONE_ANIME_STATE.Swing].Invoke();}
    public void WaveAttack(){animCallback[(int)ELDERONE_ANIME_STATE.WaveAttack].Invoke();}
    public void Dirt(){animCallback[(int)ELDERONE_ANIME_STATE.Dirt].Invoke();}
    public void AttackReady(){animCallback[(int)ELDERONE_ANIME_STATE.AttackReady].Invoke();}
    public void SpecialReady(){animCallback[(int)ELDERONE_ANIME_STATE.SpecialReady].Invoke();}
}
