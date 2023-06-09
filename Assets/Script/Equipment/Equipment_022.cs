
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_022 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public EntityAffector freezeAffector;
    private UnityAction<Entity, Entity> Projectile;

    public override void InitEquipment( int _selectIndex)
    {
        Debug.Log("InitEquipment");
        equipmentName = "얼어붙은 투구";
        this.Projectile += (_owner, _target) => {Freeze(_owner, _target);};
        if(_selectIndex == 0){
            this.EquipmentData.playerData.EntityDatas.ProjectileShootState += Projectile;
        }
    }

    public void Freeze(Entity _owner, Entity _target){
        Debug.Log("Freeze Init");
        freezeAffector.Init(_owner, _target).Modifiy();
    }
}