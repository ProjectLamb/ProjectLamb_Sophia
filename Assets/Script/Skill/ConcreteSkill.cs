using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

//이놈은 Q,E,R에 위치를 바꿀 수 있다.
// 입력은 QER이 들어온다.

// Skill[E_SkillKey.Q].Use()
// Skill[E_SkillKey.E].Use()
// Skill[E_SkillKey.R].Use()
// 그렇다면 

public abstract class ConcreteSkill : MonoBehaviour
{
    // SkillElement : 스킬 키 Q, E, R
    public string equipmentName;
    public string description;
    public ProjectileBucket projectileBucket;
    public E_SkillRank skillRank;
    private E_SkillKey assignedKey;

    [SerializedDictionary("Skill Key", "SkillElement")]
    public SerializedDictionary<E_SkillKey, SkillElement> skillElements = new SerializedDictionary<E_SkillKey, SkillElement>();
    private Entity ownerEntity;
    protected bool isReady = true;
    protected bool isInitialized = false;
    public float passedTime = 0f;
    /*
    public void Initialisze(Player _owner, E_SkillKey _key)
    {
        ownerEntity = _owner;
        projectileBucket = _owner.projectileBucket;
        assignedKey = _key;
        isInitialized = true;
    }
    public void Use(int _amount)
    {
        if (isInitialized == false) { throw new System.Exception("스킬이 초기화 되지 않음 SkillManager에서 skill.Initialize(Entity _owner); 했는지 확인"); }
        if (!isReady) return;
        List<Projectile> useProjectiles = skillElements[assignedKey].skillCarrier[skillRank];
        useProjectiles.ForEach(E => E.Initialize(ownerEntity));
        foreach (Projectile E in useProjectiles)
        {
            switch (E.carrierType)
            {
                case E_CarrierType.Attack:
                    E.transform.localScale *= PlayerDataManager.GetWeaonData().Range;
                    _amount = (int)(_amount * PlayerDataManager.GetWeaonData().DamageRatio);
                    _amount = (int)(_amount * skillElements[assignedKey].numericsArray[skillRank].amountRatio);
                    projectileBucket.ProjectileInstantiatorByDamage(ownerEntity, E, E_BucketPosition.Outer, _amount);
                    break;
                case E_CarrierType.Nutral:
                    E.projectileAffector[0].SetValue(skillElements[assignedKey].numericsArray[skillRank].ToList());
                    projectileBucket.ProjectileInstantiator(ownerEntity, E, E_BucketPosition.Inner);
                    break;
                default:
                    break;
            }
        }
        PlayerDataManager.GetPlayerData().SkillState.Invoke();
    }
    public IEnumerator CoWaitUse(float waitSecondTime)
    {
        isReady = false;
        while (passedTime <= waitSecondTime)
        {
            passedTime += Time.fixedDeltaTime;
            yield return YieldInstructionCache.WaitForSeconds(Time.fixedDeltaTime);
        }
        isReady = true;
    }

    public void WaitSkillDelay()
    {
        float waitSecondTime = skillElements[assignedKey].coolTime[skillRank];
        StartCoroutine(CoWaitUse(waitSecondTime));
    }
    */
}