using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class NeutralSkill_005 : AbstractSkill {
    //  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public PowerUpState powerUpState;

    public List<float>  NumericQ = new List<float> {0.3f, 0.5f, 0.7f};
    public float        DurationQ = 5f;
    public List<float>  NumericE = new List<float> {0.3f, 0.5f, 0.7f};
    public float        DurationE = 5f;
    public List<float>  NumericR = new List<float> {0.5f, 0.7f, 1f};
    public float        DurationR = 10f;
    


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
        skillName = "공격력 증폭";    
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.NEUTRAL;
    }

    protected override void UseQ(){
        powerUpState.Ratio = NumericQ[(int)skillRank];
        powerUpState.DurationTime = DurationQ;
        powerUpState.Init(player, player).Modifiy();
    }
    protected override void UseE(){
        powerUpState.Ratio = NumericE[(int)skillRank];
        powerUpState.DurationTime = DurationE;
        powerUpState.Init(player, player).Modifiy();
    }
    protected override void UseR(){
        powerUpState.Ratio = NumericR[(int)skillRank];
        powerUpState.DurationTime = DurationR;
        powerUpState.Init(player, player).Modifiy();
    }
}