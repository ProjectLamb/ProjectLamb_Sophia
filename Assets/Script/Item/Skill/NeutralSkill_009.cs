using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

using Sophia_Carriers;


public class NeutralSkill_009 : NeutralSkill {
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
    //public PushState          pushState;
    //public PullState          pullState;
    //public BoundedState       bloundedState;
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
        skillName = "모두 발사!";    
        coolTime?.Add(SKILL_RANK.NORMAL  , 15f);
        coolTime?.Add(SKILL_RANK.RARE    , 15f);
        coolTime?.Add(SKILL_RANK.EPIC    , 15f);
    }
    private void Start(){ }

    protected override void UseQ(){ }
    protected override void UseE(){ }
    protected override void UseR(){ }
}