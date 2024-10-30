using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ELDERONE_DASH_STATE
{
    DashEnter, DashExit, DashTerminate
}

public class SionDashAnimEventInvoker : MonoBehaviour
{
    public UnityEvent[] animCallback;

    public void DashActivate() => animCallback[(int)ELDERONE_DASH_STATE.DashEnter].Invoke();
    public void DashExit() => animCallback[(int)ELDERONE_DASH_STATE.DashExit].Invoke();
    public void DashTerminate() => animCallback[(int)ELDERONE_DASH_STATE.DashTerminate].Invoke();
}
