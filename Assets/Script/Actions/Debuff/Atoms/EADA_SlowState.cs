using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Atomic한 상태이상을 가진 EntityAffector이다. 사용하기위해 이 객체의 Affect() 매서드를 호출하면 된다
/// </br> * 이 상태이상은 플레이어를 느려지게 만드는 상태 이상이다.
/// </summary>
/// <param name="_owner">호출자의 게임오브젝트이다</param>
/// <param name="_target">영향을 받는 타겟 게임오브젝트다.</param>
/// <param name="_params">
/// * [0]: DurationTime : float </br> 
/// * [1]: SlowAmount : float
/// > * 0 ~ 1 까지의 값이고, 낮을수록 느리다.
/// </param>
/// <returns></returns>
public class EADA_SlowState : EntityAffector {
    PlayerData mPlayerData;
    float   mDurationTime;
    float   mSlowAmount;
    public EADA_SlowState(GameObject _owner, GameObject _target, object[] _params) : base(_owner, _target, _params){
        _target.TryGetComponent<PlayerData>(out mPlayerData);
        mDurationTime = (float)Params[0];
        mSlowAmount = (float)Params[1];
        this.AsyncAffectorCoroutine.Add(Coroutine());
    }
    public override void Affect() {
        this.Target.GetComponent<IAffectableEntity>().AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    IEnumerator Coroutine() {
        mPlayerData.numericData.MoveSpeed *= mSlowAmount;
        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);
        mPlayerData.numericData.MoveSpeed /= mSlowAmount;
    }
}
