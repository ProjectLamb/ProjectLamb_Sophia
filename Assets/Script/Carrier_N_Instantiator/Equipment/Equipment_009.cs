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
    //public MasterData EquipmentAddingData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;

    private UnityActionRef<float> HitState;
    public DodgeState dodgeState;
    public Player player;
    private void Awake() {
        player = GameManager.Instance.PlayerGameObject.GetComponent<Player>();
    }
    public override void InitEquipment( int _selectIndex)
    {
        HitState += (ref float _amount) => {Dodged(ref _amount);};
        equipmentName = "조명탄";
        if(_selectIndex == 0) {
            this.AddingData.playerData.EntityDatas.HitStateRef += HitState;
        }
        this.mIsInitialized = true;
    }

    //디버프를 얘가 만든다면?f

    public void Dodged(ref float amount) {
        int Luck = 5 + PlayerDataManager.GetPlayerData().Luck + PlayerDataManager.GetPlayerData().Luck;
        if(Luck >= (int)Random.Range(0, 100)){ 
            dodgeState.Dodge(ref amount);
            dodgeState.Init(this.player, this.player).Modifiy();
        }
    }
}