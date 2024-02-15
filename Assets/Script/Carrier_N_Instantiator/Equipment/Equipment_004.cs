using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_004 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public  DefenceState defenceState;

    private UnityActionRef<float> HitStateRef;
    private UnityAction<Entity, Entity> Projectile;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "디지털 파편 조각";
        HitStateRef += (ref float i) => {Defence(ref i);};
        Projectile += (Entity _owner, Entity _target) => {Defence(_owner, _target);};

        if(_selectIndex == 0) {
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.AddingData.playerData.EntityDatas.Power += (int)(readedEntityData.Power * 0.1f);
            this.AddingData.playerData.EntityDatas.HitStateRef += HitStateRef;
        }
        this.mIsInitialized = true;
    }

    public void Defence(ref float _amount) {
        defenceState.Defence(ref _amount);
    }

    public void Defence(Entity _owner, Entity _target){
        defenceState.Init(_owner, _owner).Modifiy();
    }
}