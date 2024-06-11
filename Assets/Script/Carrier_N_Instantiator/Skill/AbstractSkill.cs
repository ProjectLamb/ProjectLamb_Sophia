using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;

[System.Serializable]
public abstract class AbstractSkill : MonoBehaviour
{
    public Player                                   player;
    public string                                   skillName;
    public SKILL_RANK                               skillRank;
    public SKILL_TYPE                               skillType;
    public string                                   description;
    public bool                                     IsReady = true;
    public float                                    PassedTime = 0f;
    public SerializedDictionary<SKILL_RANK, float>  coolTime = new SerializedDictionary<SKILL_RANK, float>();

    public virtual void Init(Player _player) {
        this.player = _player;
        IsReady = true;
        PassedTime = 0;
    }

    public virtual void Use(SKILL_KEY key, int _amount){
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
    public virtual void Indicate(SKILL_KEY key){
        Indicate();
    }
    protected abstract void Indicate();
    protected abstract void UseQ();
    protected abstract void UseE();
    protected abstract void UseR();

    public async UniTask AsyncWaitUse(float _waitSecondTime)
    {
        PassedTime = 0;
        IsReady = false;
        while (PassedTime < _waitSecondTime)
        {
            PassedTime += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        IsReady = true;
    }
    public void WaitSkillDelay()
    {
        if (IsReady) { AsyncWaitUse(coolTime[skillRank]).Forget(); }
    }
}