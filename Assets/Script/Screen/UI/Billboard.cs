using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;
    private Canvas canvas;
    
    private void Awake() {
        if(!TryGetComponent<Canvas>(out canvas)) {
            throw new System.Exception("캔버스 못찾음 ... 현재 위치에 캔버스가 없으면 그럴 수 있다.");
        }
    }
    
    private void Start()
    {
        if(canvas != null) {
            mainCam = Camera.main.transform;
            canvas.worldCamera = Camera.main;
        }
        else {

        }
    }

    private void Update() {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}