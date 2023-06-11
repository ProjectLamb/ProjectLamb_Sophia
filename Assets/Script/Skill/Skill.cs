using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이놈은 Q,E,R에 위치를 바꿀 수 있다.
// 입력은 QER이 들어온다.

// Skill[E_SkillKey.Q].Use()
// Skill[E_SkillKey.E].Use()
// Skill[E_SkillKey.R].Use()
// 그렇다면 

public class Skill : MonoBehaviour {
    // SkillElement : 스킬 키 Q, E, R
    public string equipmentName;
    public string description;
    public      ProjectileBucket    projectileBucket;
    public      E_SkillRank         skillRank;
    private     E_SkillKey          assignedKey;
    public      E_SkillType         skillType;
    public      SkillElement[]      skillElements = new SkillElement[Enum.GetValues(typeof(E_SkillKey)).Length];
    private     Entity              ownerEntity;
    protected   bool                isReady = true;
    protected   bool                isInitialized = false;

    public virtual void Initialisze(Player _owner, ProjectileBucket _projectileBucket, E_SkillKey _key) {
        ownerEntity = _owner;
        projectileBucket = _projectileBucket;
        assignedKey = _key;
        isInitialized = true;
    }
    public void Use(int _amount){
        _amount = (int)(_amount * skillElements[(int)assignedKey].GetNumericByRank(skillRank));
        DynamicsCarrier instantCarrier = (DynamicsCarrier)skillElements[(int)assignedKey].GetCarrierRank(skillRank);
        instantCarrier.Initialize(ownerEntity);
        projectileBucket.CarrierInstantiator(ownerEntity, instantCarrier);
    }
    public IEnumerator CoWaitUse(float waitSecondTime){
        isReady = false;
        yield return YieldInstructionCache.WaitForSeconds(waitSecondTime);
        isReady = true;
    }
    public void WaitWeaponDelay(){
        float waitSecondTime = skillElements[(int)assignedKey].GetDurateTimeByRank(skillRank);
        StartCoroutine(CoWaitUse(waitSecondTime));
    }
}