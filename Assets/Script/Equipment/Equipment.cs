using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*********************************************************************************
*
* 먹었을떄 적용되며, 해제될떄 적용 될것
*
*********************************************************************************/
public interface IPlayerDataApplicant {
    public void ApplyData(ref PlayerData _playerData){

    }
    public void ApplyRemove(ref PlayerData _playerData){}
}
public interface IWeaponDataApplicant {
    public void ApplyData(ref WeaponData _weaponData){

    }
    public void ApplyRemove(ref WeaponData _weaponData){}
}
public interface ISkillDataApplicant {
    public void ApplyData(ref SkillData _skillData){

    }
    public void ApplyRemove(ref SkillData _skillData){}
}

public interface IActiveSelf {

}


public abstract class Equipment : MonoBehaviour{
    [Header("부품 이름")]
    [SerializeField]
    public string equipmentName;
    public string description;
    protected bool mIsApplyed;

    public virtual void Equip(ref PlayerData pd){}
    public virtual void Unequip(ref PlayerData pd){}
}