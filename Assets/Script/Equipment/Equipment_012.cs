using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class Equipment_012 : AbstractEquipment { //, IPlayerDataApplicant{
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
        Vector3 ScaleSize = new Vector3(0.7f * 0.25f,0.7f * 0.25f,0.7f * 0.25f);
        equipmentName = "핑크 덤벨";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        if(_selectIndex == 0){
            this.equipmentData.MaxHP += 10;
            this.equipmentData.Power += 10;
            _player.transform.localScale = ScaleSize;
        }
    }
}