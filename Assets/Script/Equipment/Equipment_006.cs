using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_006 : Equipment {//, IPlayerDataApplicant{
    
    [SerializeField]
    public PlayerData.Numeric playerOuterData;
    public WeaponData.Numeric weaponOuterData;
    private void Awake() {
        equipmentName = "반쯤 남은 위장크림";

        playerOuterData = new PlayerData.Numeric();
        weaponOuterData = new WeaponData.Numeric();
    }
    public PlayerData.Numeric ApplyData(ref PlayerData _playerData){
        playerOuterData.Power = _playerData.numericData.MoveSpeed * 0.1f;
        return this.playerOuterData;
    }
    public WeaponData.Numeric ApplyData(ref WeaponData _weaponData){
        weaponOuterData.WeaponDelay = -_weaponData.numericData.WeaponDelay * 0.05f;
        return this.weaponOuterData;
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