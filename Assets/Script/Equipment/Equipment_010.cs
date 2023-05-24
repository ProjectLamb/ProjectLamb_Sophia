using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_010 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction AttackState;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "슉슈슉..";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.AttackState += () => {ChargeAttack();};
        this.player = _player;
        if(_selectIndex == 0){
            this.equipmentData.AttackSpeed += _player.playerData.AttackSpeed * 0.2f;
            this.equipmentData.AttackState += AttackState;
        }
        this.mIsInitialized = true;
    }

    //디버프를 얘가 만든다면?
    public void ChargeAttack() {
        IEntityAddressable entityAddressable = this.player.GetComponent<IEntityAddressable>();
        new ChargeState(this.player).Modifiy(entityAddressable);
    }
}