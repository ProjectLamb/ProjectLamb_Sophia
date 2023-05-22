using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_011 : AbstractEquipment { //, IPlayerDataApplicant{
    
    private UnityAction AttackState;
    bool isCritical = false;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "노란색 레고블럭";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        this.player = _player;
        _player.pipelineData.MaxHP  -= 10;
        _player.pipelineData.MoveSpeed -= _player.playerData.MoveSpeed * 0.1f;
        _player.pipelineData.AttackSpeed += _player.playerData.AttackSpeed;
        _player.playerData.AttackState += Critical;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.pipelineData.MaxHP  += 10;
        _player.pipelineData.MoveSpeed += _player.playerData.MoveSpeed * 0.1f;
        _player.pipelineData.AttackSpeed -= _player.playerData.AttackSpeed;
        _player.playerData.AttackState -= Critical;
    }

    //디버프를 얘가 만든다면?
    public void Critical() {
        if(this.player.playerData.Luck + 5 < (int)Random.Range(0, 100)){ 
            if(isCritical == false) {
                this.player.pipelineData.Power += this.player.playerData.Power * 5; 
                isCritical = true;
            }
        }
        else {
            if(isCritical == true){ 
                this.player.pipelineData.Power -= this.player.playerData.Power * 5;
                isCritical = false;
            }
        }
    }
}