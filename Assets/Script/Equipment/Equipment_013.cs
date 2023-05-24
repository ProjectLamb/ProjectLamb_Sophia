using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_013 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "노동자의 망치";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        this.Projectile += (GameObject obj) => {Knockback(obj);};   
        if(_selectIndex == 0){
            _player.playerData.ProjectileShootState += Projectile;
        }
    }

    //디버프를 얘가 만든다면?
    public void Knockback(GameObject _target) {
        IEntityAddressable entityAddressable = _target.GetComponent<IEntityAddressable>();
        new KnockBackState(this.player.gameObject, _target).Modifiy(entityAddressable);
    }
}