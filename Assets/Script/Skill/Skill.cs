using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [HideInInspector]
    public PlayerData playerData;
    
    [HideInInspector]
    public WeaponData weaponData;
    public SkillData skillData;

    public GameObject skillEffect;

    bool mIsReady = true;
    IEnumerator mCoWaitUse;

    private void Awake() {
        //if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
        if(!TryGetComponent<SkillData>(out skillData)) {Debug.Log("컴포넌트 로드 실패 : SkillData");}
    }
    public void Use(){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);

        Instantiate(skillEffect, transform.position, transform.rotation).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
    }
    IEnumerator CoWaitUse(){
        yield return YieldInstructionCache.WaitForSeconds(1f);
        mIsReady = true;
    }
}