using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_eventEmitter : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter eventEmitter;
    private void Awake() {
        if(!TryGetComponent<FMODUnity.StudioEventEmitter>(out eventEmitter)){
            Debug.Log("컴포넌트가 안붙어 버림");
        };
    }
    private void Start(){
        eventEmitter.Play();
    }
}
