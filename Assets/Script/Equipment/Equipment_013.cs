using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_013 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public EntityAffector KnockBackAffector;
    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "노동자의 망치";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        this.Projectile += (_owner, _target) => {Knockback(_owner, _target);};   
        if(_selectIndex == 0){
            this.equipmentData.ProjectileShootState += Projectile;
        }
    }

    //디버프를 얘가 만든다면?
    public void Knockback(Entity _owner,Entity _target) {
        KnockBackAffector.Init(this.player, _target);
        KnockBackAffector.Modifiy((IAffectable)_target);
    }
}