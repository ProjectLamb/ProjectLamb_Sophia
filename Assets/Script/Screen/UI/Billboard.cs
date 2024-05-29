using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public int Flip = 1;
    private Camera mainCameraRef;
    private void Awake() {
        mainCameraRef = Camera.main;
        if(TryGetComponent<Canvas>(out Canvas canvas)) {
            canvas.worldCamera = mainCameraRef;
        }
        Flip = 1;
    }
    void LateUpdate()
    {
        SetFront();
    }
    
    [ContextMenu("Set Front")]
    public void SetFront() { transform.forward = mainCameraRef.transform.forward * Flip; }
}