using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Equipment_005 : AbstractEquipment { //, IPlayerDataApplicant{
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
        equipmentName = "동력전달장치";

        if(_selectIndex == 0) {
            this.AddingData.playerData.EntityDatas.Power += (PlayerDataManager.GetPlayerData().Gear / 10);
        }
        this.mIsInitialized = true;
    }
}