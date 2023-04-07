using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_001 : Equipment{
    public PlayerData outerData;
    private void Awake() {
        this.EquipmentName = "식어버린_피자_한조각";
        this.playerData = new PlayerData();
        playerData.FakePlayerDataContructor();
        this.weaponData = new WeaponData();
        this.skillData = new SkillData();
        playerData.numericData.MaxHP = 10;
        playerData.numericData.MoveSpeed = -outerData.numericData.MoveSpeed * 0.05f;
        ToString();
    }
    public override void Adaptation(PlayerData _input){
        _input.numericData.MaxHP += this.playerData.numericData.MaxHP;
        _input.numericData.MoveSpeed += this.playerData.numericData.MoveSpeed;
    }

    public override string ToString() => $"playerData : {JsonUtility.ToJson(playerData)} weaponData : {JsonUtility.ToJson(weaponData)}";
}