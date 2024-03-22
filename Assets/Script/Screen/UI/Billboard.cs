using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;
    
    private void Start()
    {
        mainCam = Camera.main.transform;
        if(TryGetComponent<Canvas>(out Canvas canvas)) {
            canvas.worldCamera = Camera.main;
        }
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}