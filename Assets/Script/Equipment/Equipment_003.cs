
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_003 : AbstractEquipment { //, IPlayerDataApplicant{
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "뒤집어진 양말";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }
    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.pipelineData.Power += (int)(_player.playerData.Power * 0.1f);
        _player.pipelineData.MaxHP -= 15;
    }
    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.Power -= (int)(_player.playerData.Power * 0.1f);
        _player.pipelineData.MaxHP += 15;
    }
}