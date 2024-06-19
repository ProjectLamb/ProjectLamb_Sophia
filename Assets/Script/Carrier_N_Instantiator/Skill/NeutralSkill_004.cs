using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class NeutralSkill_004 : AbstractSkill {
//  public string                                   skillName;
//  public string                                   description;
//  public  SKILL_RANK                              skillRank;
//  public  Player                                  player;
//  protected bool                                  isReady = true;
//  protected float                                 passedTime = 0f;
//  public  bool                                    IsReady = true;
//  public  float                                   PassedTime = 0f;
//  public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

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
    public List<float>  NumericQ = new List<float> {0.3f, 0.5f, 0.7f};
    public List<float>  NumericE = new List<float> {0.3f, 0.5f, 0.7f};
    public List<float>  NumericR = new List<float> {0.5f, 0.7f, 1f};
    public SkillManager skillManager;

    private void Awake() {
        this.skillName = "시간은 누구에게나 평등하지";
        //coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        //coolTime?.Add(SKILL_RANK.RARE    , 15f);
        //coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    public override void Init(Player _player)
    {
        base.Init(_player);
        skillType = SKILL_TYPE.NEUTRAL;
        skillManager = _player.skillManager;
    }
    protected override void UseQ(){
        SKILL_RANK targetSkillRank = this.skillManager.skills[(int)SKILL_KEY.E].skillRank;
        float coolDownAmount = this.skillManager.skills[(int)SKILL_KEY.E].coolTime[targetSkillRank];
        this.skillManager.skills[(int)SKILL_KEY.E].PassedTime += coolDownAmount * NumericQ[(int)skillRank];
    }
    protected override void UseE(){
        SKILL_RANK targetSkillRank = this.skillManager.skills[(int)SKILL_KEY.Q].skillRank;
        float coolDownAmount = this.skillManager.skills[(int)SKILL_KEY.Q].coolTime[targetSkillRank];
        this.skillManager.skills[(int)SKILL_KEY.Q].PassedTime += coolDownAmount * NumericE[(int)skillRank];
    }
    protected override void UseR(){
        SKILL_RANK targetSkillRankQ = this.skillManager.skills[(int)SKILL_KEY.Q].skillRank;
        float coolDownAmountQ = this.skillManager.skills[(int)SKILL_KEY.Q].coolTime[targetSkillRankQ];
        this.skillManager.skills[(int)SKILL_KEY.Q].PassedTime += coolDownAmountQ * NumericR[(int)skillRank];
        
        SKILL_RANK targetSkillRankE = this.skillManager.skills[(int)SKILL_KEY.E].skillRank;
        float coolDownAmountE = this.skillManager.skills[(int)SKILL_KEY.E].coolTime[targetSkillRankE];
        this.skillManager.skills[(int)SKILL_KEY.E].PassedTime += coolDownAmountE * NumericR[(int)skillRank];
    }

}