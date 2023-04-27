/*
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class EADA_SlowState : EntityAction {
    PlayerData mPlayerData;
    //EnemyData mEnemyData;
    float mDurationTime;
    float mDamageAmount;

    
    EADA_DotState(ref GameObject _owner, ref GameObject _target, object _parameters) {
        _owner.TryGetComponent<PlayerData>(out mPlayerData);
        mDurationTime = (float)_param.durateTime;
        mDamageAmount = (float)_param.damageAmont;
    }

    public override void ToAffect(ref GameObject _self, Object _parameters){
        EADA_DotState(_self, _self, _parameters);
        
    }
    public override void ToAffect(ref GameObject _owner, ref GameObject _target, Object _parameters){
        EADA_DotState(_owner, _target, _parameters);
    }
}
*/