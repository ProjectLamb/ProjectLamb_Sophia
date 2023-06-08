using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_006 : AbstractEquipment { //, IPlayerDataApplicant{
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

    public override void InitEquipment(int _selectIndex)
    {
        equipmentName = "반쯤남은위장크림";
        if(_selectIndex == 0){
            this.equipmentData.playerData.EntityDatas.MoveSpeed   += PlayerDataManager.BasePlayerData.EntityDatas.MoveSpeed * 0.1f;
            this.equipmentData.playerData.EntityDatas.AttackSpeed += PlayerDataManager.BasePlayerData.EntityDatas.AttackSpeed * 0.05f;
        }
        this.mIsInitialized = true;
    }
}