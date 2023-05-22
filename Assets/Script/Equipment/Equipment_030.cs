using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_030 : AbstractEquipment { //, IPlayerDataApplicant{
    private UnityAction<GameObject> Projectile;
    bool isCritical = false;
    private void Awake() {
        InitEquipment();
    }
    public override void InitEquipment()
    {
        equipmentName = "러다이트 운동 지령서";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (GameObject obj) => {Execution(obj);};
    }

    public override void Equip(Player _player, int _selectIndex) {
        if(!this.mIsInitialized){InitEquipment();}        
        this.player = _player;
        _player.playerData.ProjectileShooter += Projectile;
    }

    public override void Unequip(Player _player, int _selectIndex){
        _player.playerData.ProjectileShooter -= Projectile;
    }

    public void Execution(GameObject _target){
        if(this.player.playerData.Luck + 5 < (int)Random.Range(0, 100)){ 
            IPipelineAddressable pipelineAddressable = _target.GetComponent<IPipelineAddressable>();
            new ExecutionState(_target).Modifiy(pipelineAddressable);
        }
    }
}