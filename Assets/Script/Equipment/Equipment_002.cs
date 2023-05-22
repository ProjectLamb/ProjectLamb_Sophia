using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_002 : AbstractEquipment { //, IPlayerDataApplicant{
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment(){
        equipmentName = "뼈치킨";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }
    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.pipelineData.MaxHP -= 10;
        _player.pipelineData.MoveSpeed += _player.playerData.MoveSpeed * 0.05f;
    }
    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.MaxHP += 10;
        _player.pipelineData.MoveSpeed -= _player.playerData.MoveSpeed * 0.05f;
    }
}