using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CombatEffect : MonoBehaviour {
    public GameObject hitEffect;
    public Collider hitBox;
    public Action mOnHitAction;

    PlayerData playerData;
    WeaponData weaponData;

    IEnumerator mCoEnableOff;

    public UnityEvent HitEvents;


    //ParticleSystem effectParticle;

    private void Awake() {
        //if(!TryGetComponent<ParticleSystem>(out effectParticle)){Debug.Log("컴포넌트 로드 실패 : PlayerData");}
        if(!TryGetComponent<Collider>(out hitBox)){Debug.Log("컴포넌트 로드 실패 : PlayerData");}
        hitBox.enabled = true;
        
        //HitEvents = GameManager.Instance?.globalEvent.OnHitEvents;
    }
    private void Start() {
        Debug.Log(this.ToString());
        mCoEnableOff = CoEnableOff();
        StartCoroutine(mCoEnableOff);        
    }

    IEnumerator CoEnableOff() {
        yield return YieldInstructionCache.WaitForSeconds(0.15f);
        hitBox.enabled = false;
    }

    public void SetDatas(PlayerData _pd, WeaponData _wd){
        this.playerData = _pd;
        this.weaponData = _wd;
    }

    //public override string ToString() => $"playerData : {JsonUtility.ToJson(playerData)} weaponData : {JsonUtility.ToJson(weaponData)}";
    //public void SetDatas(PlayerData _pd, WeaponData _wd, SkillData _sd){}
}