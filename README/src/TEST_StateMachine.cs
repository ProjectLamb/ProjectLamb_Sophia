using System.Collections;
using UnityEngine;

namespace TEST
{
    public abstract class StateMachine : MonoBehaviour{
        protected State state;
        public void SetState(State _state){
            State = _state;
            StartCoroutine(State.Start());
        }
    }    
}