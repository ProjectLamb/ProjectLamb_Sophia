using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_008 : AbstractEquipment { //, IPlayerDataApplicant{
    //public string equipmentName;
    //public string description;
    //public Sprite sprite;
    //[SerializeField]
    //public MasterData equipmentData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;
    public EntityAffector sternAffector;

    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "황소용 올가미";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        this.Projectile += (Entity _owner, Entity _target) => {Sturn(_owner, _target);};
        if(_selectIndex == 0){
            this.equipmentData.ProjectileShootState += Projectile;
        }
        this.mIsInitialized = true;
    } 
    
    //디버프를 얘가 만든다면?
    public void Sturn(Entity _owner, Entity _target) {
        sternAffector.Init(_owner, _target).Modifiy();
    }
}