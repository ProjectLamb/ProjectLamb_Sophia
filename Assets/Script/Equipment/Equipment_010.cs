using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_010 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction AttackState;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "슉슈슉..";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.AttackState += () => {ChargeAttack();};
    }

    public override void Equip(Player _player, int _selectIndex) {
        this.player = _player;
        if(_selectIndex == 0){
            if(!this.mIsInitialized){InitEquipment();}
            _player.pipelineData.AttackSpeed += _player.playerData.AttackSpeed * 0.2f;
        }
        if(_selectIndex == 1){
            if(!this.mIsInitialized){InitEquipment();}
            _player.playerData.AttackState += AttackState;
        }
    }

    public override void Unequip(Player _player, int _selectIndex){
        if(_selectIndex == 0){
            _player.pipelineData.AttackSpeed -= _player.playerData.AttackSpeed * 0.2f;
        }
        if(_selectIndex == 1){
            _player.playerData.AttackState -= AttackState;
        }
    }

    //디버프를 얘가 만든다면?
    public void ChargeAttack() {
        IPipelineAddressable pipelineAddressable = this.player.GetComponent<IPipelineAddressable>();
        new ChargeState(this.player.gameObject).Modifiy(pipelineAddressable);
    }
}