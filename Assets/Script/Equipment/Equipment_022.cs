
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_022 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    bool isCritical = false;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "얼어붙은 투구";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (GameObject obj) => {Freeze(obj);};
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}        
        this.player = _player;
        _player.playerData.ProjectileShooter += Projectile;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.playerData.ProjectileShooter -= Projectile;
    }

    public void Freeze(GameObject _target){
        IPipelineAddressable pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
        new FreezeState(_target).Modifiy(pipelineAddressable);
    }
}