using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment_001 : AbstractEquipment {
    public float multipyMoveSpeed;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "식어버린 피자 한조각";

        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.mIsInitialized = true;
    }
    public override void Equip(Player _player, int _selectIndex){
        if(!this.mIsInitialized){InitEquipment();}
        _player.addingData.MaxHP += 10;
        _player.addingData.MoveSpeed -= _player.playerData.MoveSpeed * 0.05f;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.addingData.MaxHP -= 10;
        _player.addingData.MoveSpeed += _player.playerData.MoveSpeed * 0.05f;
    }
}