using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_010 : AbstractEquipment { //, IPlayerDataApplicant{
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
    public EntityAffector chargeAttackAffector;
    private UnityAction AttackState;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "슉슈슉..";
        this.AttackState += () => {ChargeAttack();};
        if(_selectIndex == 0){
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.AddingData.playerData.EntityDatas.AttackSpeed += readedEntityData.AttackSpeed * 0.2f;
            this.AddingData.playerData.EntityDatas.AttackState += AttackState;
        }
        this.mIsInitialized = true;
    }

    //디버프를 얘가 만든다면?
    public void ChargeAttack() {
        //chargeAttackAffector.Init(this.player, this.player).Modifiy();
    }
}