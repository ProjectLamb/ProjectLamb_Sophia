using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_004 : Equipment {//, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric outerData;
    private void Awake() {
        equipmentName = "디지털 파편 조각";

        outerData = new PlayerData.Numeric();
    }
    public PlayerData.Numeric ApplyData(ref PlayerData _playerData) {
        outerData.Power = 0.1f * _playerData.numericData.Power;
        outerData.Defense = 10;
        return outerData;
    }
    /*
    public void ApplyData(ref PlayerData _playerData){
        if(this.mIsApplyed != false) return;
        this.mIsApplyed = true;
        _playerData.numericData.Power *= 1.1f;
    }

    public void ApplyRemove(ref PlayerData _playerData) {
        if(this.mIsApplyed != true) return;
        this.mIsApplyed = false;
        _playerData.numericData.Power /= 1.1f;
    }

    public override void Equip(ref PlayerData pd){
        ApplyData(ref pd);
    }
    public override void Unequip(ref PlayerData pd){
        ApplyRemove(ref pd);
    }

    public override string ToString() => "";
    */
}