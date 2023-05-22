using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_006 : AbstractEquipment { //, IPlayerDataApplicant{
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "반쯤남은위장크림";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }
    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.pipelineData.MoveSpeed   += _player.playerData.MoveSpeed * 0.1f;
        _player.pipelineData.AttackSpeed += _player.playerData.AttackSpeed * 0.05f;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.MoveSpeed   -= _player.playerData.MoveSpeed * 0.1f;
        _player.pipelineData.AttackSpeed -= _player.playerData.AttackSpeed * 0.05f;
    }
}