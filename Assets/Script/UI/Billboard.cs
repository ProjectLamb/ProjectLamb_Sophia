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
        mainCam = Camera.main.transform;
        canvas.worldCamera = Camera.main;
    }

    private void Update() {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}