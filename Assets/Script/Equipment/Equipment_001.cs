using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_001 : Equipment { //, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric outerData;
    private void Awake() {
        equipmentName = "식어버린 피자 한조각";
        outerData = new PlayerData.Numeric();
    }

    public PlayerData.Numeric ApplyData(ref PlayerData _playerData){
        outerData.MaxHP = 10;
        outerData.MoveSpeed = -_playerData.numericData.MoveSpeed * 0.05f;
        return outerData;
    }

    /*
    public void ApplyData(ref PlayerData _playerData){
        if(this.mIsApplyed != false) return;
        this.mIsApplyed = true;
        _playerData.numericData.MaxHP += 10;
        _playerData.numericData.MoveSpeed *= 0.95f;
    }

    public void ApplyRemove(ref PlayerData _playerData){
        if(this.mIsApplyed != true) return;
        this.mIsApplyed = false;
        _playerData.numericData.MaxHP -= 10;
        _playerData.numericData.MoveSpeed /= 0.95f;
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