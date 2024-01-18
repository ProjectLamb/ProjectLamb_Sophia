using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;
    private Canvas canvas;
    
    private void Awake() {
        TryGetComponent<Canvas>(out canvas);
    }
    
    private void Start()
    {
        if(canvas != null) canvas.worldCamera = Camera.main;
        mainCam = Camera.main.transform;
    }

    private void Update() {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}