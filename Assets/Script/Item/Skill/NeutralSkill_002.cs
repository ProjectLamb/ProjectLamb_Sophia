using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;
using Sophia_Carriers;

public class NeutralSkill_002 : NeutralSkill {
//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public OnHitState   onHitState;
    public SternState   sternState;
    public Weapon       weapon;
    public List<float>  NumericQ = new List<float> {1.2f, 1.4f, 1.6f};
    public List<float>  NumericE = new List<float> {0.5f, 1f, 1.5f};
    public List<float>  NumericAttackRatioR = new List<float> {1.1f, 1.2f, 1.3f};
    public List<float>  NumericSternR = new List<float> {0.3f, 0.5f, 0.7f};

//  public void Use(SKILL_KEY key){
//      switch(key) {
//          case SKILL_KEY.Q : UseQ();
//              break;
//          case SKILL_KEY.E : UseE();
//              break;
//          case SKILL_KEY.R : UseR();
//              break;
//      }
//  }
    private void Awake() {
        this.skillName = "머리가 어질어질";
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        weapon = _player.weaponManager.weapon;
    }

    protected override void UseQ(){
        onHitState.Ratio = NumericQ[(int)skillRank];
        onHitState.Init(player, player);
        Projectile onHitProjectile = onHitState.ActivateOnHit(
            weapon.AttackProjectiles[0]
        );
        ParticleSystem.MainModule particleModule = onHitProjectile.GetComponent<ParticleSystem>().main;
        particleModule.startColor = Color.red;
        weapon.OnHitProjectiles.Enqueue(onHitProjectile);
        weapon.ChangeState(WEAPON_STATE.ON_HIT);
    }

    protected override void UseE(){
        sternState.DurationTime = NumericE[(int)skillRank];
        onHitState.Ratio = 1;
        onHitState.Init(player, player);
        Projectile onHitProjectile = onHitState.ActivateOnHitByAffectors(
            weapon.AttackProjectiles[0], 
            new List<EntityAffector>{sternState}
        );
        ParticleSystem.MainModule particleModule = onHitProjectile.GetComponent<ParticleSystem>().main;
        particleModule.startColor = Color.yellow;
        weapon.OnHitProjectiles.Enqueue(onHitProjectile);
        weapon.ChangeState(WEAPON_STATE.ON_HIT);
    }
    protected override void UseR(){
        sternState.DurationTime     = NumericSternR[(int)skillRank];
        onHitState.Ratio            = NumericAttackRatioR[(int)skillRank];
        onHitState.Init(player, player);
        Projectile onHitProjectile = onHitState.ActivateOnHitByAffectors(
            weapon.AttackProjectiles[0], 
            new List<EntityAffector>{sternState}
        );
        ParticleSystem.MainModule particleModule = onHitProjectile.GetComponent<ParticleSystem>().main;
        particleModule.startColor = Color.magenta;
        weapon.OnHitProjectiles.Enqueue(onHitProjectile);
        weapon.ChangeState(WEAPON_STATE.ON_HIT);
    }
}