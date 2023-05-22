using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_018 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "노동자의 망치";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (GameObject obj) => {Knockback(obj);};   
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}
        this.player = _player;
        _player.playerData.ProjectileShootState += Projectile;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.playerData.ProjectileShootState -= Projectile;
    }

    //디버프를 얘가 만든다면?
    public void Knockback(GameObject _target) {
        IPipelineAddressable pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        new KnockBackState(this.player.gameObject, _target).Modifiy(pipelineAddressable);
    }
}