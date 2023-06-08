using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_030 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public  EntityAffector excuteAffector;
    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "러다이트 운동 지령서";

        this.Projectile += (Entity _owner, Entity _target) => {Execution(_owner, _target);};
        if(_selectIndex == 0){
            this.equipmentData.playerData.EntityDatas.ProjectileShootState += Projectile;
        }
        this.mIsInitialized = true;
    }

    public void Execution(Entity _owner, Entity _target){
        excuteAffector.Init(_owner, _target).Modifiy();
    }
}