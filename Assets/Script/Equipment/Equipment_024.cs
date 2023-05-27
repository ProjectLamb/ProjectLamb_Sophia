
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
    //public MasterData equipmentData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;
    public EntityAffector poisonAffector;
    private UnityAction<Entity, Entity> Projectile;

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "독개구리";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (_owner, _target) => {Poison(_owner, _target);};
        this.player = _player;
        if(_selectIndex == 0){
            this.equipmentData.ProjectileShootState += Projectile;
        }
    }

    public void Poison(Entity _owner, Entity _target){
        poisonAffector.Init(_owner, _target);
        poisonAffector.Modifiy((IAffectable)_target);
    }
}