using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class TimeHandleCompoenet : MonoBehaviour, ITimeAffectable
{
    private bool IsPause = false;
    public void Pause() {
        IsPause = true;
    }
    public void Play() {
        IsPause = false;
    }
}