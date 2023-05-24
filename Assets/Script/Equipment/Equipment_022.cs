
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_022 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "얼어붙은 투구";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (GameObject obj) => {Freeze(obj);};
        this.player = _player;
        if(_selectIndex == 0){
            _player.playerData.ProjectileShootState += Projectile;
        }
    }

    public void Freeze(GameObject _target){
        IEntityAddressable entityAddressable = _target.GetComponent<IEntityAddressable>();
        new FreezeState(_target).Modifiy(entityAddressable);
    }
}