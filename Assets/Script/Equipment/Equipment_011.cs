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
    //public MasterData equipmentData;
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
            this.equipmentData.playerData.EntityDatas.MaxHP -= 10;
            this.equipmentData.playerData.EntityDatas.MoveSpeed -= PlayerDataManager.BasePlayerData.EntityDatas.MoveSpeed * 0.1f;
            this.equipmentData.playerData.EntityDatas.AttackSpeed += PlayerDataManager.BasePlayerData.EntityDatas.AttackSpeed * 0.1f;
            this.equipmentData.playerData.EntityDatas.AttackState += AttackState;
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