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


    public override void InitEquipment( int _selectIndex)
    {
        Vector3 ScaleSize = new Vector3(7f * 1.25f,7f * 1.25f,7f * 1.25f);
        equipmentName = "핑크 덤벨";
        if(_selectIndex == 0){
            this.equipmentData.playerData.EntityDatas.MaxHP += 10;
            this.equipmentData.playerData.EntityDatas.Power += 10;
            GameManager.Instance.playerGameObject.transform.localScale = ScaleSize;
        }
    }
}