using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이놈은 Q,E,R에 위치를 바꿀 수 있다.
// 입력은 QER이 들어온다.

// Skill[E_SkillKey.Q].Use()
// Skill[E_SkillKey.E].Use()
// Skill[E_SkillKey.R].Use()

// 그렇다면 

public class Skill : MonoBehaviour
{   
    [SerializeField]
    private SkillData mBaseSkillData;
    public SkillData BaseSkillData {get {return BaseSkillData;}}
    
    [SerializeField]
    public SkillData SkillData;
    bool mIsReady = true;
    IEnumerator mCoWaitUse;

    private void Awake() {
        //if(!TryGetComponent<WeaponData>(out weaponData)) {Debug.Log("컴포넌트 로드 실패 : WeaponData");}
        //if(!TryGetComponent<SkillData>(out skillData)) {Debug.Log("컴포넌트 로드 실패 : SkillData");}
    }
    public void Start(){

    }
    public void Use(int _amount){
        if(!mIsReady) return;
        mIsReady = false;
        mCoWaitUse = CoWaitUse();
        StartCoroutine(mCoWaitUse);
        
        //Projectile 생성 단, 전달데이터는 Owner이 누구인지, _amount만 전달하는것으로
        //Instantiate(skillEffect, transform.position, transform.rotation).GetComponent<CombatEffect>().SetDatas(this.playerData, this.weaponData);
    }
    IEnumerator CoWaitUse(){
        yield return YieldInstructionCache.WaitForSeconds(1f);
        mIsReady = true;
    }
}