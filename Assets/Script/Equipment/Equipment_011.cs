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
    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "노란색 레고블럭";
        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        AttackState += () => {Critical();};
        if(_selectIndex == 0){
            this.equipmentData.MaxHP -= 10;
            this.equipmentData.MoveSpeed -= _player.playerData.MoveSpeed * 0.1f;
            this.equipmentData.AttackSpeed += _player.playerData.AttackSpeed * 0.1f;
            originBasePower = _player.playerData.Power;
            originAddPower = _player.equipmentManager.AddingData.Power;
            this.equipmentData.AttackState += AttackState;
        }
    }

    //디버프를 얘가 만든다면?
    public void Critical() {
        int Luck = this.player.playerData.Luck + this.player.equipmentManager.AddingData.Luck + 5;
        if(Luck < (int)Random.Range(0, 100)){ 
            if(isCritical == false) {
                this.player.playerData.Power = originBasePower * 5; 
                isCritical = true;
            }
        }
        else {
            if(isCritical == true){ 
                this.player.playerData.Power = originBasePower; 
                isCritical = false;
            }
        }
    }
}