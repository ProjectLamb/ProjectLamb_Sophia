using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_012 : AbstractEquipment { //, IPlayerDataApplicant{
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "핑크 덤벨";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.pipelineData.MaxHP   += 10;
        _player.pipelineData.Power   += 10;
        _player.transform.localScale += _player.transform.localScale * 0.25f;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.MaxHP   -= 10;
        _player.pipelineData.Power   -= 10;
        _player.transform.localScale -= _player.transform.localScale * 0.25f;
    }
}