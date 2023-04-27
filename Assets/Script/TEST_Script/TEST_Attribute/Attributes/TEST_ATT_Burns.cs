using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Sandbox_Attribute {
    protected GameObject activator;
    protected GameObject target;
    protected bool mIsActivate;
    protected int mCounts = 0;
    protected string mAttributeName;
    protected IEnumerator mCoroutine;
    protected float mDurationTime;

    public Sandbox_Attribute(string _name, float _durationTime){
        this.mAttributeName = _name;
        this.mDurationTime = _durationTime;
    }
}

public class TEST_ATT_Burns : Sandbox_Attribute {
    public TEST_ATT_Burns(string _name, float _durationTime) : base(_name, _durationTime){
        mAttributeName = "Burns";
    }

    public MeshRenderer meshRenderer;

    public void Activate(){
    }

    IEnumerator HandleDurate(){
        if(mIsActivate == true) {mCounts++; yield break;}
        mIsActivate = true;
        float passedTime = mDurationTime;
        float gapTime = 0.5f;
        while(passedTime > 0){
            passedTime -= gapTime;
            yield return YieldInstructionCache.WaitForSeconds(gapTime);
        }
        mIsActivate = false;
    }
}