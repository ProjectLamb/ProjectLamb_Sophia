using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{   
    [SerializeField]
    public SkillData newSkillData;

    EntityData entityData;

    bool mIsReady = true;
    IEnumerator mCoWaitUse;

    private void Awake() {
        //if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
        //if(!TryGetComponent<SkillData>(out skillData)) {Debug.Log("컴포넌트 로드 실패 : SkillData");}
    }
    public void Start(){
        entityData = GetComponentInParent<IEntityAddressable>().GetEntityData();

    }
    public void Use(MasterData AddingData){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);

        //Instantiate(skillEffect, transform.position, transform.rotation).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
    }
    IEnumerator CoWaitUse(){
        yield return YieldInstructionCache.WaitForSeconds(1f);
        mIsReady = true;
    }
}