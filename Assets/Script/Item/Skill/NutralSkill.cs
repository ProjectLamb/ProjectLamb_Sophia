using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public abstract class NutralSkill : AbstractSkill {
    public override void Init(Player _player) {
        base.Init(_player);
    }
    public override void Use(SKILL_KEY key, int _amount){
        if(!this.IsReady) {return;}
        switch(key) {
            case SKILL_KEY.Q : UseQ();
                WaitSkillDelay();
                break;
            case SKILL_KEY.E : UseE();
                WaitSkillDelay();
                break;
            case SKILL_KEY.R : UseR();
                WaitSkillDelay();
                break;
        }
    }
}