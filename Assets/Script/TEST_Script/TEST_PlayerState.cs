using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TEST_PlayerState 
{
    protected TEST_Player mPlayer;
    protected IEnumerator mCoWait;
    protected YieldInstruction mWait;
    protected float mDurationTime;
    protected bool IsActivate {get; set;}
    public TEST_PlayerState(GameObject _gameObject, float _durationTime) {
        this.mPlayer = _gameObject.GetComponent<TEST_Player>();
    }
    public abstract void Invoke();
}

//Burn = 0 ,Poisend ,Bleed ,Contracted ,Slow ,Confused ,Fearing ,Stern ,Bounded
public class TEST_BurnState : TEST_PlayerState{
    public TEST_BurnState(GameObject _gameObject, float _durationTime) : base(_gameObject, _durationTime){
    }
    public override void Invoke(){
        mCoWait = this.Cor();
    }
    public IEnumerator Cor()
    {
        float passedTime = mDurationTime;
        //어떤 효과 집어넣기
        while(passedTime > 0){
            passedTime -= Time.deltaTime;
            yield return null;
        }
    }
}