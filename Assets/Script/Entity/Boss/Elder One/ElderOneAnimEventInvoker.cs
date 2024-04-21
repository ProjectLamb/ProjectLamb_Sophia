using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ELDERONE_ANIME_STATE
{
    Swing,
}

public class ElderOneAnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void Swing(){animCallback[(int)ELDERONE_ANIME_STATE.Swing].Invoke();}
}
