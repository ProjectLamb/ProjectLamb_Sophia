using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SionDashVFX : MonoBehaviour {
    
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _sionDashVfx;

    private bool IsActiaved;

    public void DashActivate() {
        if(IsActiaved) return;
        IsActiaved = true;
        gameObject.SetActive(IsActiaved);
        _animator.SetTrigger("Enter");

    }

    public void DashExit() {
        _animator.SetTrigger("Exit");
    }
    
    public void DashTerminate() {
        if(!IsActiaved) return;
        IsActiaved = false;
        gameObject.SetActive(IsActiaved);
    }
}