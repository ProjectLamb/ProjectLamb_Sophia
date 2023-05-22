using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_007 : AbstractEquipment { //, IPlayerDataApplicant{
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "유통기한 지난 전투 식량";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }
    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.pipelineData.MaxHP -= 10;
        _player.pipelineData.Tenacity = 0.5f;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.MaxHP += 10;
        _player.pipelineData.Tenacity = 0;
    }
}