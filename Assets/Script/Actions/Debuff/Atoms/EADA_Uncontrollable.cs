using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Atomic한 상태이상을 가진 EntityAffector이다. 사용하기위해 이 객체의 Affect() 매서드를 호출하면 된다
/// </br> * 이 상태이상은 모든 키보드 인풋이 안먹는 상태이상이다.
/// </summary>
/// <param name="_owner">호출자의 게임오브젝트이다</param>
/// <param name="_target">영향을 받는 타겟 게임오브젝트다.</param>
/// <param name="_params">* [0]: DurationTime : float</param>
/// <returns></returns>
public class EADA_Inattackable : EntityAffector {
    PlayerData mPlayerData;
    float   mDurationTime;
    public EADA_Inattackable(GameObject _owner, GameObject _target, object[] _params) : base(_owner, _target, _params){
        _target.TryGetComponent<PlayerData>(out mPlayerData);
        mDurationTime = (float)Params[0];
        this.AsyncAffectorCoroutine.Add(Coroutine());
    }
    public override void Affect() {
        this.Target.GetComponent<IAffectableEntity>().AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    IEnumerator Coroutine() {
        PlayerController.IsAttackAllow = false;
        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);
        PlayerController.IsAttackAllow = true;
    }
}
