using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_009 : AbstractEquipment { //, IPlayerDataApplicant{
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

    private UnityActionRef<int> HitState;

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "조명탄";
        HitState += (ref int _amount) => {Dodged(ref _amount);};
        if(_selectIndex == 0) {
            this.equipmentData.playerData.EntityDatas.HitStateRef += HitState;
        }
        this.mIsInitialized = true;
    }

    //디버프를 얘가 만든다면?

    public void Dodged(ref int amount) {
        int Luck = 5 + PlayerDataManager.GetPlayerData().Luck + PlayerDataManager.GetPlayerData().Luck;
        if(Luck >= (int)Random.Range(0, 100)){ 
            amount = 0;
        }
    }
}