using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_006 : AbstractEquipment { //, IPlayerDataApplicant{
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "반쯤남은위장크림";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        if(_selectIndex == 0){
            this.equipmentData.MoveSpeed   += _player.playerData.MoveSpeed * 0.1f;
            this.equipmentData.AttackSpeed += _player.playerData.AttackSpeed * 0.05f;
        }
        this.mIsInitialized = true;
    }
}