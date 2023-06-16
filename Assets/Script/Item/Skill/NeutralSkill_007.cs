using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class NeutralSkill_007 : NeutralSkill {

//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public MoveFasterState      moveFasterState; 
    protected bool              isReady = true;
    public List<float>          NumericQ = new List<float> {1.2f, 1.3f, 1.4f};
    public float                DurationQ = 5f;
    public List<float>          NumericE = new List<float> {1.2f, 1.3f, 1.4f};
    public float                DurationE = 5f;
    public List<float>          NumericR = new List<float> {1.4f, 1.5f, 1.7f};
    public float                DurationR = 10f;

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
        skillName = "이동속도 증폭";    
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    protected override void UseQ(){
        moveFasterState.DurationTime = DurationQ;
        moveFasterState.Ratio = NumericQ[(int)skillRank];
        moveFasterState.Init(player,player).Modifiy();
    }
    protected override void UseE(){
        moveFasterState.DurationTime = DurationE;
        moveFasterState.Ratio = NumericE[(int)skillRank];
        moveFasterState.Init(player,player).Modifiy();
    }
    protected override void UseR(){
        moveFasterState.DurationTime = DurationR;
        moveFasterState.Ratio = NumericR[(int)skillRank];
        moveFasterState.Init(player,player).Modifiy();
    }
}