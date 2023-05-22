using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_004 : AbstractEquipment { //, IPlayerDataApplicant{
    
    private UnityActionRef<int> HitState;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "디지털 파편 조각";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        HitState += (ref int i) => {MoreDamage(ref i);};
    }
    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.addingData.Power += (int)(_player.playerData.Power * 0.1f);
        _player.playerData.HitStateRef += HitState;
    }
    public override void Unequip(Player _player, int _selectIndex){
        _player.addingData.Power -= (int)(_player.playerData.Power * 0.1f);
        _player.playerData.HitStateRef -= HitState;
    }
    public void MoreDamage(ref int _amount) {
        _amount += (int)(_amount * 0.1f);
    }
}