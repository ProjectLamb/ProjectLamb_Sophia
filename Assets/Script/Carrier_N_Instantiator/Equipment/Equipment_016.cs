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
    //public MasterData EquipmentAddingData;
    //protected Player player;
    //public UnityAction EquipState;
    //public UnityAction UnequipState;
    //public UnityAction UpdateState;
    //public bool mIsInitialized = false;
    public MoveFasterState fasterAffector;
    private UnityAction EnemyDie;
    public Player player;
    private void Awake() {
        player = GameManager.Instance.PlayerGameObject.GetComponent<Player>();
    }

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "조잡한 황금뱃지";
        EnemyDie += () => {Faster();};
        GameManager.Instance.GlobalEvent.OnEnemyDieEvent.Add(EnemyDie);

        this.mIsInitialized = true;
    }

    public void Faster() {
        fasterAffector.Init(this.player, this.player).Modifiy();
    }
}