using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_007 : AbstractEquipment { //, IPlayerDataApplicant{
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

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "유통기한 지난 전투 식량";
        if(_selectIndex == 0) {
            this.EquipmentData.playerData.EntityDatas.MaxHP -= 10;
            this.EquipmentData.playerData.EntityDatas.Tenacity += 0.5f;
        }
        this.mIsInitialized = true;
    }
}