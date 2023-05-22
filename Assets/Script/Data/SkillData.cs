using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillData {
    [field : SerializeField]public E_SkillType SkillType;
    [field : SerializeField]public string SkillName;
    [field : SerializeField]public SkillInfo[] SkillInfos;
    [field : SerializeField]public string SkillDescription;
    [field : SerializeField]public Projectile SkillProjectile;
    [field : SerializeField]public UnityAction UseState;
    [field : SerializeField]public UnityAction ChangeState;
    [field : SerializeField]public UnityAction LevelUpState;
    
    public SkillData(ScriptableObjSkillData _skillScriptable) {
        SkillType = _skillScriptable.skillType;
        SkillName = _skillScriptable.skillName;
        SkillDescription = _skillScriptable.skillDescription;
        SkillInfos = _skillScriptable.skillInfo;
        SkillProjectile = _skillScriptable.projectile;
        
        UseState = () => {};
        ChangeState = () => {};
        LevelUpState = () => {};
    }
}