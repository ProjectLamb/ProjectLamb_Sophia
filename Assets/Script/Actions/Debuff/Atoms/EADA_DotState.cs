using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EADA_DotState : EntityAction {
    PlayerData mPlayerData;
    //EnemyData mEnemyData;
    float mDurationTime;
    int mDamageAmount;

    
    public EADA_DotState(ref GameObject _owner, ref GameObject _target, object[] _parameters): base(ref _owner, ref _target, _parameters){
        _owner.TryGetComponent<PlayerData>(out mPlayerData);
        mDurationTime = (float)_parameters[0];
        mDamageAmount = (int)_parameters[1];
    }

    public override void ToAffect(ref GameObject _self, Object _parameters){
        IEnumerator DotCoroutine() {
            float passedTime = 0;
            while(mDurationTime > passedTime){
                passedTime += 0.5f;
                mPlayerData.numericData.CurHP -= mDamageAmount;
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
            }
        }
    }
    public override void ToAffect(ref GameObject _owner, ref GameObject _target, Object _parameters){
        IEnumerator DotCoroutine() {
            float passedTime = 0;
            while(mDurationTime > passedTime){
                passedTime += 0.5f;
                mPlayerData.numericData.CurHP -= mDamageAmount;
                yield return YieldInstructionCache.WaitForSeconds(0.5f);
            }
        }
    }
}