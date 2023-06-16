using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class NeutralSkill_003 : NeutralSkill {
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
    private void Awake(){
        skillName = "빠르게 탈출하기";
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }
    public override void Init(Player _player){base.Init(_player); }

    protected override void UseQ(){return;}
    protected override void UseE(){return;}
    protected override void UseR(){return;}
}