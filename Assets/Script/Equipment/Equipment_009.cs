using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_009 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityActionRef<int> HitState;

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "조명탄";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        HitState += (ref int _amount) => {Dodged(ref _amount);};
        if(_selectIndex == 0) {
            this.equipmentData.HitStateRef += HitState;
        }
        this.mIsInitialized = true;
    }

    //디버프를 얘가 만든다면?
    public void Dodged(ref int amount) {
        int Luck = 5 + this.player.equipmentManager.AddingData.Luck + this.player.playerData.Luck;
        if(Luck >= (int)Random.Range(0, 100)){ 
            amount = 0;
        }
    }
}