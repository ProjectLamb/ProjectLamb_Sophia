using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillData {
    public E_SkillType SkillType;
    public string SkillName;
    public SkillInfo[] SkillInfos;
    public string SkillDescription;

    public Projectile SkillProjectile;
    
    public UnityAction UseState;
    public UnityAction ChangeState;
    public UnityAction LevelUpState;
    
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