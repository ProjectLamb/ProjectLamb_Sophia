using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment_002 : AbstractEquipment { //, IPlayerDataApplicant{
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
        equipmentName = "뼈치킨";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        if(_selectIndex == 0) {
            this.equipmentData.MaxHP -= 10;
            this.equipmentData.MoveSpeed += _player.playerData.MoveSpeed * 0.05f;
        }
        this.mIsInitialized = true;
    }
}