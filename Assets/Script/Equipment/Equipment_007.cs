using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_007 : AbstractEquipment { //, IPlayerDataApplicant{
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "유통기한 지난 전투 식량";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        
        if(_selectIndex == 0) {
            this.equipmentData.MaxHP -= 10;
            this.equipmentData.Tenacity += 0.5f;
        }
        this.mIsInitialized = true;
    }
}