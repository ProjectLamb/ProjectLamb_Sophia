using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment_001 : AbstractEquipment {
    // public string equipmentName;
    // public string description;
    // public Sprite sprite;
    // [SerializeField]
    // public MasterData EquipmentAddingData;
    // protected Player player;
    // public UnityAction EquipState;
    // public UnityAction UnequipState;
    // public UnityAction UpdateState;
    // public bool mIsInitialized = false;

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "식어버린 피자 한조각";
        if(_selectIndex == 0) {
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.AddingData.playerData.EntityDatas.MaxHP += 10;
            this.AddingData.playerData.EntityDatas.MoveSpeed -= readedEntityData.MoveSpeed * 0.05f;
        }
        this.mIsInitialized = true;
    }
}