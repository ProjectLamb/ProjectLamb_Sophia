using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Sophia_Carriers;

public class NeutralSkill_008 : AbstractSkill {
//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    //public ProjectileGenState   PGState;
    public BurnState            burnState;
    public ParticleProjectile   ParticleProjectileQ;
    public ParticleProjectile   ParticleProjectileE;
    public ParticleProjectile   ParticleProjectileR;
    protected bool              isReady = true;
    public List<float>          NumericQ = new List<float> {0.1f, 0.15f, 0.2f};
    public List<float>          NumericE = new List<float> {0.1f, 0.15f, 0.2f};
    public List<float>          NumericR = new List<float> {0.2f, 0.3f, 0.4f};

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
        skillName = "바닥은 용암이야";    
        //coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        //coolTime?.Add(SKILL_RANK.RARE    , 15f);
        //coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.NEUTRAL;
    }
    protected override void Indicate(){
        
    }
    protected override void UseQ(){
        // float damage = PlayerDataManager.GetEntityData().Power * NumericQ[(int)skillRank] + 1;
        // ParticleProjectile useProjectile = ParticleProjectileQ.CloneParticleProjectile();
        // useProjectile.Init(player);
        // useProjectile.ProjecttileDamage = 0;
        // useProjectile.ParticleMainModule.duration = 0.5f;
        // useProjectile.ParticleMainModule.startLifetime = 20f;
        // useProjectile.ParticleEmissionModule.rateOverTime = (1f / 0.25f);
        // burnState.DurationTime = 3f;
        // burnState.Damage = damage;
        // useProjectile.projectileAffector.Add(burnState);
        // player.carrierBucket.CarrierTransformPositionning(player, useProjectile);
    }
    protected override void UseE(){

    }
    protected override void UseR(){

    }
}