using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_030 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "러다이트 운동 지령서";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (GameObject obj) => {Execution(obj);};
        this.player = _player;
        if(_selectIndex == 0){
            _player.playerData.ProjectileShootState += Projectile;
        }
    }

    public void Execution(GameObject _target){
        int Luck = this.player.playerData.Luck + this.player.equipmentManager.AddingData.Luck + 5;
        if(Luck + 5 < (int)Random.Range(0, 100)){ 
            IEntityAddressable entityAddressable = _target.GetComponent<IEntityAddressable>();
            new ExecutionState(_target).Modifiy(entityAddressable);
        }
    }
}