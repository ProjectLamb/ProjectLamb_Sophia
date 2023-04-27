using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_002 : Equipment, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric outerData;
    private void Awake() {
        equipmentName = "뼈치킨";
    }
    public void ApplyData(ref PlayerData _playerData){
        if(this.mIsApplyed != false) return;
        this.mIsApplyed = true;
        _playerData.numericData.MaxHP -= 10;
        _playerData.numericData.MoveSpeed *= 1.05f;
    }
    public void ApplyRemove(ref PlayerData _playerData){
        if(this.mIsApplyed != true) return;
        this.mIsApplyed = false;
        _playerData.numericData.MaxHP += 10;
        _playerData.numericData.MoveSpeed /= 1.05f;
    }

    public override void Equip(ref PlayerData pd){
        ApplyData(ref pd);
    }
    public override void Unequip(ref PlayerData pd){
        ApplyRemove(ref pd);
    }

    public override string ToString() => "";
}