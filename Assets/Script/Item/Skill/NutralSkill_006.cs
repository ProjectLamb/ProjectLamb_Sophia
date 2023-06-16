using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class NutralSkill_006 : NutralSkill {

//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();
    public BarrierState     barrierState;
    public List<float>  NumericQ = new List<float> {0.2f, 0.3f, 0.4f};
    public float        DurationQ = 5f;
    public List<float>  NumericE = new List<float> {0.2f, 0.3f, 0.4f};
    public float        DurationE = 5f;
    public List<float>  NumericR = new List<float> {0.4f, 0.5f, 0.7f};
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
        skillName = "보호막 증폭";
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    protected override void UseQ(){
        barrierState.DurationTime = DurationQ;
        barrierState.Ratio = NumericQ[(int)skillRank];
        barrierState.Init(player,player).Modifiy();
    }
    protected override void UseE(){
        barrierState.DurationTime = DurationE;
        barrierState.Ratio = NumericE[(int)skillRank];
        barrierState.Init(player,player).Modifiy();
    }
    protected override void UseR(){
        barrierState.DurationTime = DurationR;
        barrierState.Ratio = NumericR[(int)skillRank];
        barrierState.Init(player,player).Modifiy();
    }
}