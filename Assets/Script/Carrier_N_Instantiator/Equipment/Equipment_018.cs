using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Equipment_018 : AbstractEquipment {
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
        equipmentName = "만화책";

        if(_selectIndex == 0) {
            MasterData.MaxStaminaReferer.AddCalculator(new Sophia.DataSystem.StatCalculator(1, Sophia.E_STAT_CALC_TYPE.Add));
            MasterData.MaxStaminaReferer.RecalculateStat();
        }
        this.mIsInitialized = true;
    }
}