using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_008 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "황소용 올가미";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};   
        this.Projectile += (GameObject obj) => {Sturn(obj);};
        if(_selectIndex == 0){
            this.equipmentData.ProjectileShootState += Projectile;
        }
        this.mIsInitialized = true;
    } 
    
    //디버프를 얘가 만든다면?
    public void Sturn(GameObject _target) {
        IEntityAddressable entityAddressable = _target.GetComponent<IEntityAddressable>();
        new SternState(_target).Modifiy(entityAddressable);
    }
}