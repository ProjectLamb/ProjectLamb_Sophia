using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia {
    public class AudioMarker : MonoBehaviour
    {
        public FMODPlus.FMODAudioSource OnFootStep;
        public FMODPlus.FMODAudioSource OnSwing;

        public void FootSteps(){
            OnFootStep.Play();
        }
        public void WeaponeSwing(){
            OnSwing.Play();
        }
    }
}