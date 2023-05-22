using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_008 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "황소용 올가미";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};   
        this.Projectile += (GameObject obj) => {Sturn(obj);};
    } 

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        _player.playerData.ProjectileShootState += Projectile;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.playerData.ProjectileShootState -= Projectile;
    }

    //디버프를 얘가 만든다면?
    public void Sturn(GameObject _target) {
        IPipelineAddressable pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        new SternState(_target).Modifiy(pipelineAddressable);
    }
}