using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

/*
public class NeutralSkill_008 : NeutralSkill {
//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public ProjectileGenState   PGState;
    public BurnState            burnState;
    public Projectile           ProjectileQ;
    public Projectile           ProjectileE;
    public Projectile           ProjectileR;
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
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    private void Start(){
        ProjectileQ.projectileAffector.Add(burnState);
        ProjectileE.projectileAffector.Add(burnState);
        ProjectileR.projectileAffector.Add(burnState);
    }

    protected override void UseQ(){
        float damage = PlayerDataManager.GetEntityData().Power * NumericQ[(int)skillRank];
        ProjectileQ.Initialize(player);
        ProjectileQ.projectileAffector.Last().SetValue(new List<float> {3f, damage});
        ProjectileQ.destroyTime = 20f;
        PGState.DurationTime        = 5f;
        PGState.RepeatTimeInterval  = 0.25f;
        PGState.projectile = ProjectileQ;
        PGState.bucketPoisiotn = BUCKET_POSITION.OUTER;
        PGState.Init(player, player).Modifiy();
    }
    protected override void UseE(){
        float damage = PlayerDataManager.GetEntityData().Power * NumericE[(int)skillRank];
        ProjectileE.Initialize(player);
        ProjectileE.projectileAffector.Last().SetValue(new List<float> {3f, damage});
        ProjectileE.destroyTime     = 5f;
        PGState.DurationTime        = 20f;
        PGState.RepeatTimeInterval  = 0.25f;
        PGState.projectile = ProjectileE;
        PGState.bucketPoisiotn = BUCKET_POSITION.OUTER;
        PGState.Init(player, player).Modifiy();
    }
    protected override void UseR(){
        float damage = PlayerDataManager.GetEntityData().Power * NumericR[(int)skillRank];
        ProjectileR.Initialize(player);
        ProjectileR.projectileAffector.Last().SetValue(new List<float> {20f, damage});
        ProjectileR.destroyTime     = 20f;
        PGState.DurationTime        = 1f;
        PGState.RepeatTimeInterval  = 1f;
        PGState.projectile = ProjectileR;
        PGState.bucketPoisiotn = BUCKET_POSITION.OUTER;
        PGState.Init(player, player).Modifiy();
    }
}
*/