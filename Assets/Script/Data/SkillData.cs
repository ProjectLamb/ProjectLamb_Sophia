using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillData {
    [field : SerializeField]public E_SkillType SkillType;
    [field : SerializeField]public string SkillName;
    [field : SerializeField]public SkillInfo[] SkillInfos;
    [field : SerializeField]public string SkillDescription;
    [field : SerializeField]public List<Projectile> SkillProjectile;
    [field : SerializeField]public UnityAction UseState;
    [field : SerializeField]public UnityAction ChangeState;
    [field : SerializeField]public UnityAction LevelUpState;
    
    public SkillData() {
        SkillProjectile = new List<Projectile>();
        UseState = () => {};
        ChangeState = () => {};
        LevelUpState = () => {};
    }
}