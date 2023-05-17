using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_007 : Equipment {//, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric playerOuterData;
    private void Awake() {
        equipmentName = "유통기한 지난 전투식량";
        playerOuterData = new PlayerData.Numeric();
    }
    public void ApplyData(ref PlayerData _playerData){
        playerOuterData.MaxHP = 10;
    }
    public void ApplyActions() {

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