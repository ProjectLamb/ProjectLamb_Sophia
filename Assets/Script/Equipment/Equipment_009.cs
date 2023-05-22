using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_009 : AbstractEquipment { //, IPlayerDataApplicant{
    
    private UnityActionRef<int> HitState;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "조명탄";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        HitState += (ref int _amount) => {Dodged(ref _amount);};
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        this.player = _player;
        _player.playerData.HitStateRef += HitState;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.playerData.HitStateRef -= HitState;
    }

    //디버프를 얘가 만든다면?
    public void Dodged(ref int amount) {
        if(this.player.playerData.Luck + 5 >= (int)Random.Range(0, 100)){ 
            amount = 0;
        }
    }
}