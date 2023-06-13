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

    public  CriticalState critialAffector;
    private UnityAction Attack;
    public Player player;
    private void Awake() {
        player = GameManager.Instance.playerGameObject.GetComponent<Player>();
    }

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "노란색 레고블럭";
        Attack += () => {Critical();};
        if(_selectIndex == 0){
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.AddingData.playerData.EntityDatas.AttackState += Attack;
        }
    }

    //디버프를 얘가 만든다면?
    public void Critical() {
        int Luck = 5 + PlayerDataManager.GetPlayerData().Luck + PlayerDataManager.GetPlayerData().Luck;
        if(Luck >= (int)Random.Range(0, 100)){ 
            critialAffector.Init(this.player, this.player).Modifiy();
        }
    }
}