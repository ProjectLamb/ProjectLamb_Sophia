using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillData {
    [field : SerializeField]public E_SkillType SkillType;
    [field : SerializeField]public E_SkillKey CurrentSkillKey;
    [field : SerializeField]public string SkillName;
    [field : SerializeField]public SkillInfo[] SkillInfos;
    [field : SerializeField]public string SkillDescription;
    [field : SerializeField]public List<Projectile> SkillProjectile;
    [field : SerializeField]public UnityAction SkillUseState;
    [field : SerializeField]public UnityAction SkillChangeState;
    [field : SerializeField]public UnityAction SkillLevelUpState;
    
    public SkillData() {
        SkillProjectile = new List<Projectile>();
        SkillUseState = () => {};
        SkillChangeState = () => {};
        SkillLevelUpState = () => {};
    }
}