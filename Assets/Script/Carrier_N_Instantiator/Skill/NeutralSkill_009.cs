using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

using Sophia_Carriers;


public class NeutralSkill_009 : AbstractSkill {
//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();
    //public PushState          pushState;
    //public PullState          pullState;
    //public BoundedState       bloundedState;
    protected bool              isReady = true;
    public List<float>          DurationQ = new List<float> {3f,5f,10f};
    public List<float>          DurationE = new List<float> {3f,5f,10f};
    public List<float>          DurationR = new List<float> {3f,5f,10f};


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
    public BlackHoleState               pullState;
    public BlackHoleState               pushState;
    public BoundedState                 boundState;
    public TriggerExplosion             TriggerQ;
    public TriggerExplosion             TriggerE;
    public TriggerExplosion             TriggerR;

    private void Awake() {
        skillName = "모두 발사!";    
        //coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        //coolTime?.Add(SKILL_RANK.RARE    , 15f);
        //coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.NEUTRAL;
    }

    
    protected override void UseQ(){
        TriggerExplosion useTrigger = TriggerQ.CloneTriggerExplosion();
        useTrigger.Init(player);
        useTrigger.triggerAffectors.Add(pullState);
        pullState.DurationTime = DurationQ[(int)skillRank];
        pullState.Direction = -0.5f;
        player.carrierBucket.CarrierTransformPositionning(player, useTrigger);
    }
    protected override void UseE(){
        TriggerExplosion useTrigger = TriggerE.CloneTriggerExplosion();
        useTrigger.Init(player);
        useTrigger.triggerAffectors.Add(pushState);
        pushState.DurationTime = DurationE[(int)skillRank];
        pushState.Direction = 0.5f;
        player.carrierBucket.CarrierTransformPositionning(player, useTrigger);
    }
    protected override void UseR(){
        TriggerExplosion useTrigger = TriggerR.CloneTriggerExplosion();
        useTrigger.Init(player);
        boundState.DurationTime = DurationR[(int)skillRank];
        useTrigger.triggerAffectors.Add(boundState);
        player.carrierBucket.CarrierTransformPositionning(player, useTrigger);
    }
}