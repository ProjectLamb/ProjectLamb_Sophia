
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_024 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public EntityAffector poisonAffector;
    private UnityAction<Entity, Entity> Projectile;

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "독개구리";
        this.Projectile += (_owner, _target) => {Poison(_owner, _target);};
        if(_selectIndex == 0){
            this.EquipmentData.playerData.EntityDatas.ProjectileShootState += Projectile;
        }
    }

    public void Poison(Entity _owner, Entity _target){
        poisonAffector.Init(_owner, _target).Modifiy();
    }
}