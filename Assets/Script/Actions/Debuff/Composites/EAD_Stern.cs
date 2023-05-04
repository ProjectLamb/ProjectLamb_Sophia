using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Atomic한 상태이상을 가진 EntityAffector이다. 사용하기위해 이 객체의 Affect() 매서드를 호출하면 된다
/// </summary>
/// <param name="_owner">호출자의 게임오브젝트이다</param>
/// <param name="_target">영향을 받는 타겟 게임오브젝트다.</param>
/// <param name="_params">
/// * [0]: DurationTime : float </br> 
/// * [1]: DamageAmount : int
/// * [2]: mTargetMeshRender : MeshRenderer
/// </param>
/// <returns></returns>
public class EAD_SternState : EntityAffector
{
    PlayerData mPlayerData;
    PlayerVisualData mPlayerVisualData;

    float mDurationTime;
    float mSlowAmount;
    Material mSkin;
    ParticleSystem mParticle;

    public EAD_SternState(GameObject _owner, GameObject _target, object[] _params) : base(_owner, _target, _params)
    {
        if (this.Params == null) { Debug.LogError("0: 유지시간 1:도트뎀 을 적어서 보내야함"); }
        _target.TryGetComponent<PlayerData>(out mPlayerData);
        _target.TryGetComponent<PlayerVisualData>(out mPlayerVisualData);

        mDurationTime = (float)Params[0];
        mSlowAmount = (float)Params[1];
        mSkin = (Material)Params[2];
        //mParticle     = (ParticleSystem)Params[3];

        this.AsyncAffectorCoroutine.Add(VisualActivate());
        this.AsyncAffectorCoroutine.Add(Uncontrollable());
        this.AsyncAffectorCoroutine.Add(Unmovable());
    }
    public override void Affect()
    {
        this.Target.GetComponent<IAffectableEntity>().AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    IEnumerator Uncontrollable()
    {
        PlayerController.IsAttackAllow = false;
        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);
        PlayerController.IsAttackAllow = true;
    }

    IEnumerator Unmovable()
    {
        PlayerController.IsMoveAllow = false;
        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);
        PlayerController.IsMoveAllow = true;
    }

    IEnumerator VisualActivate()
    {
        mPlayerData.attributeData.mDebuffState[(int)E_DebuffState.Stern]++;

        mPlayerVisualData.skinModulator.SetSkinSets(1, mSkin);
        mPlayerVisualData.particleModulator.ActivateParticle(mParticle, mDurationTime);

        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);

        mPlayerVisualData.skinModulator.SetSkinSets(1, null);
        mPlayerData.attributeData.mDebuffState[(int)E_DebuffState.Stern]--;
    }
}