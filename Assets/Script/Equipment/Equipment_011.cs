using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_011 : AbstractEquipment { //, IPlayerDataApplicant{
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

    
    private UnityAction AttackState;
    int originBasePower;
    int originAddPower;
    bool isCritical = false;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "노란색 레고블럭";
        AttackState += () => {Critical();};
        if(_selectIndex == 0){
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.EquipmentData.playerData.EntityDatas.MaxHP -= 10;
            this.EquipmentData.playerData.EntityDatas.MoveSpeed -=    readedEntityData.MoveSpeed * 0.1f;
            this.EquipmentData.playerData.EntityDatas.AttackSpeed +=  readedEntityData.AttackSpeed * 0.1f;
            this.EquipmentData.playerData.EntityDatas.AttackState += AttackState;
        }
    }

    //디버프를 얘가 만든다면?
    public void Critical() {
        //int Luck = PlayerDataManager.GetPlayerData().Luck + 5;
        //if(Luck < (int)Random.Range(0, 100)){ 
        //    if(isCritical == false) {
        //        PlayerDataManager.GetPlayerData().player. Power = originBasePower * 5; 
        //        isCritical = true;
        //    }
        //}
        //else {
        //    if(isCritical == true){ 
        //        PlayerDataManager.GetPlayerData().Power = originBasePower; 
        //        isCritical = false;
        //    }
        //}
    }
}