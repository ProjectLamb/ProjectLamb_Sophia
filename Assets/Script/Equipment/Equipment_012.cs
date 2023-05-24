using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_012 : AbstractEquipment { //, IPlayerDataApplicant{

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        Vector3 ScaleSize = new Vector3(0.7f * 0.25f,0.7f * 0.25f,0.7f * 0.25f);
        equipmentName = "핑크 덤벨";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        if(_selectIndex == 0){
            this.equipmentData.MaxHP += 10;
            this.equipmentData.Power += 10;
            _player.transform.localScale = ScaleSize;
        }
    }
}