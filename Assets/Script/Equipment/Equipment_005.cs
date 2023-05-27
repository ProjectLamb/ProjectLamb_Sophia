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
    //public MasterData equipmentData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "동력전달장치";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};

        if(_selectIndex == 0) {
            this.equipmentData.Power += (_player.playerData.Gear / 10);
        }
        this.mIsInitialized = true;
    }
}