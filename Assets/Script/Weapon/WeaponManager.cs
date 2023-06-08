using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour {
    public Weapon weapon;
    public PlayerDataManager PDM;
    public UnityEvent OnChangeEvent; // -> 이놈을 서브스크라이브 해야된다.
    private void Awake() {
        if(PDM == null) { throw new System.Exception("Player가 null임 인스펙터 확인 ㄱㄱ"); }   
    }
    private void Start() {
        //OnChangeEvent.
    }
    public void AssignWeapon(Weapon _weapon){
        this.weapon = _weapon;
        //OnChangeEvent.Invoke();
    }
}