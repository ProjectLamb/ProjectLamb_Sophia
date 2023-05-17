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

    public PlayerData playerData;
    WeaponData weaponData;

    IEnumerator mCoEnableOff;

    public UnityEvent HitEvents;

    //ParticleSystem effectParticle;

    public Material mSkin;
    public ParticleSystem mParticle;
    public bool isUseDynamicSystem;

    public Dictionary<Affector_PlayerState ,EntityAffector> Modifiers;

    private void Awake() {
        //if(!TryGetComponent<ParticleSystem>(out effectParticle)){Debug.Log("컴포넌트 로드 실패 : PlayerData");}
        if(!TryGetComponent<Collider>(out hitBox)){Debug.Log("컴포넌트 로드 실패 : PlayerData");}
        hitBox.enabled = true;
        this.playerData = GameManager.Instance.playerGameObject.GetComponent<PlayerData>();
        //HitEvents = GameManager.Instance?.globalEvent.OnHitEvents;
        Modifiers = new Dictionary<Affector_PlayerState, EntityAffector>();
        if(isUseDynamicSystem) {
            GameObject obj = GameManager.Instance.playerGameObject;
            Modifiers.Add(Affector_PlayerState.Attack, new TEST_EAD_PoisonState(null, obj, new object[] {3f, 1, mSkin, mParticle}));
        }
    }
    private void Start() {
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

    public void SendDynamics(){

    }

    public int SendDamage(){
        return (int)(playerData.numericData.Power * weaponData.numericData.DamageRatio);
    }

    //public override string ToString() => $"playerData : {JsonUtility.ToJson(playerData)} weaponData : {JsonUtility.ToJson(weaponData)}";
    //public void SetDatas(PlayerData _pd, WeaponData _wd, SkillData _sd){}
}