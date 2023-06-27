using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using Sophia_Carriers;
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

    public  OnHitState                  onHitState;
    public  Player                      player;
    private Weapon                      weapon;    
    private UnityAction<Entity, Entity> Projectile;
    private void Awake() {
        player = GameManager.Instance.PlayerGameObject.GetComponent<Player>();
        weapon = player.weaponManager.weapon;
    }

    public override void InitEquipment( int _selectIndex)
    {
        equipmentName = "노란색 레고블럭";
        this.Projectile += (Entity _owner, Entity _target) => {OnHit(_owner, _target);};
        if(_selectIndex == 0){
            EntityData readedEntityData = PlayerDataManager.BasePlayerData.EntityDatas;
            this.AddingData.playerData.EntityDatas.ProjectileShootState += Projectile;
        }
    }

    //디버프를 얘가 만든다면?
    public void OnHit(Entity _owner, Entity _target) {
        int Luck = 5 + PlayerDataManager.GetPlayerData().Luck;
        int Threshold = (int)Random.Range(0, 100);
        if(Luck >= Threshold){
            onHitState.Ratio = 5;
            onHitState.Init(_owner, _target);
            Projectile onHitProjectile = onHitState.ActivateOnHit(weapon.AttackProjectiles[0]);
            weapon.OnHitProjectiles.Enqueue(onHitProjectile);
            player.weaponManager.weapon.ChangeState(WEAPON_STATE.ON_HIT);
        }
    }
}

/*
언제 시작될까?
시작하는 방법은 이벤트, 직접실행
    OnHit.Init(this.owner, this.owner).Modifier();
    다음 공격이 

언제 꺼질까?
    꺼지는 방법은 뭘까?
    owner을 제외한 엔티티가 GetDamaged할때,
*/