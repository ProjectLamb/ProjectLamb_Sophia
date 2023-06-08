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
    //public MasterData equipmentData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;

    
    private UnityActionRef<int> HitStateRef;
    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "디지털 파편 조각";
        HitStateRef += (ref int i) => {Defence(ref i);};

        if(_selectIndex == 0) {
            this.equipmentData.playerData.EntityDatas.Power += (int)(PlayerDataManager.BasePlayerData.EntityDatas.Power * 0.1f);
            this.equipmentData.playerData.EntityDatas.HitStateRef += HitStateRef;
        }
        this.mIsInitialized = true;
    }

    public void Defence(ref int _amount) {
        _amount += (int)(_amount * 0.1f); // 디펜스의 반대는 더 많이 맞는다는것으로
    }
}