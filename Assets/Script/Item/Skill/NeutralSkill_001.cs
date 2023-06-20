using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class NeutralSkill_001 : AbstractSkill {
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
    public MoveFasterState  moveFasterState;
    public List<float>      NumericQ = new List<float> {0.15f, 0.25f, 0.35f};
    public float            DurationQ = 5f;
    public List<float>      NumericE = new List<float> {0.15f, 0.25f, 0.35f};
    public float            DurationE = 5f;
    public List<float>      NumericBarrierR = new List<float> {0.1f, 0.2f, 0.3f};
    public List<float>      NumericMoveSpeedR = new List<float> {0.1f, 0.2f, 0.3f};
    public float            DurationBarrierR = 5f;
    public float            DurationMoveSpeedR = 10f;

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
        skillName = "봄 바람을 타고";    
        //coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        //coolTime?.Add(SKILL_RANK.RARE    , 15f);
        //coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }
    public override void Init(Player _player) {
        base.Init(_player);
        skillType = SKILL_TYPE.NEUTRAL;
    }

    protected override void UseQ(){
        barrierState.DurationTime = DurationQ;
        barrierState.Ratio = NumericQ[(int)skillRank];
        barrierState.Init(player,player).Modifiy();
    }
    protected override void UseE(){
        moveFasterState.DurationTime = DurationE;
        moveFasterState.Ratio = NumericE[(int)skillRank];
        moveFasterState.Init(player,player).Modifiy();
    }
    protected override void UseR(){
        barrierState.DurationTime = DurationBarrierR;
        barrierState.Ratio = NumericBarrierR[(int)skillRank];
        barrierState.Init(player,player).Modifiy();
        moveFasterState.DurationTime = DurationMoveSpeedR;
        moveFasterState.Ratio = NumericMoveSpeedR[(int)skillRank];
        moveFasterState.Init(player,player).Modifiy();
    }
}