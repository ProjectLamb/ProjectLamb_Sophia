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
    //public MasterData EquipmentAddingData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;
    [SerializeField] public  StunState StunAffector;

    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "황소용 올가미";
        this.Projectile += (Entity _owner, Entity _target) => {Stun(_owner, _target);};
        if(_selectIndex == 0){
            this.AddingData.playerData.EntityDatas.ProjectileShootState += Projectile;
        }
        this.mIsInitialized = true;
    } 
    
    //디버프를 얘가 만든다면?
    public void Stun(Entity _owner, Entity _target) {
        StunAffector.Init(_owner, _target).Modifiy();
    }
}