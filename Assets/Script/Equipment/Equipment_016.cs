using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment_016 : AbstractEquipment {
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
    public EntityAffector fasterAffector;
    private UnityAction EnemyDie;

    public override void InitEquipment(Player _player, int _selectIndex)
    {
        equipmentName = "조잡한 황금뱃지";

        this.EquipState = () => {};
        this.UnequipState = () => {};
        this.UpdateState = () => {};
        this.player = _player;
        this.EnemyDie += () => {Faster();};
        if(_selectIndex == 0) {
            GameManager.Instance.globalEvent.OnEnemyDieEvent.AddListener(this.EnemyDie);
        }
        this.mIsInitialized = true;
    }

    public void Faster() {
        fasterAffector.Init(this.player, this.player).Modifiy();
    }
}