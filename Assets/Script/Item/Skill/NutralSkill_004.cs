using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AYellowpaper.SerializedCollections;

public class NutralSkill_004 : NutralSkill {
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

    private void Awake() {
        this.skillName = "시간은 누구에게나 평등하지";
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }

    protected override void UseQ(){return;}
    protected override void UseE(){return;}
    protected override void UseR(){return;}

}