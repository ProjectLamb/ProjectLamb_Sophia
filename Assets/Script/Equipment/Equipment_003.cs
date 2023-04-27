using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_003 : Equipment, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric outerData;
    private void Awake() {
        equipmentName = "뒤집어진 양말";
    }
    public void ApplyData(ref PlayerData _playerData){
        if(this.mIsApplyed != false) return;
        this.mIsApplyed = true;
        _playerData.numericData.Power *= 1.1f;
        _playerData.numericData.MaxHP -= 15;
    }
    public void ApplyRemove(ref PlayerData _playerData){
        if(this.mIsApplyed != true) return;
        this.mIsApplyed = false;
        _playerData.numericData.Power /= 1.1f;
        _playerData.numericData.MaxHP += 15;
    }


    public override void Equip(ref PlayerData pd){
        ApplyData(ref pd);
    }
    public override void Unequip(ref PlayerData pd){
        ApplyRemove(ref pd);
    }

    public override string ToString() => "";
}