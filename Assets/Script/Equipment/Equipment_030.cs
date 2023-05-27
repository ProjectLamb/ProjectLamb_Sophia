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
    public EntityAffector excuteAffector;
    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "러다이트 운동 지령서";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.Projectile += (_owner, _target) => {Execution(_owner, _target);};
        this.player = _player;
        if(_selectIndex == 0){
            _player.playerData.ProjectileShootState += Projectile;
        }
    }

    public void Execution(Entity _owner, Entity _target){
        int Luck = this.player.playerData.Luck + this.player.equipmentManager.AddingData.Luck + 5;
        if(Luck + 5 < (int)Random.Range(0, 100)){ 
            IEntityAddressable entityAddressable = _target.GetComponent<IEntityAddressable>();
            excuteAffector.Init(_owner, _target);
            excuteAffector.Modifiy(_target);
        }
    }
}