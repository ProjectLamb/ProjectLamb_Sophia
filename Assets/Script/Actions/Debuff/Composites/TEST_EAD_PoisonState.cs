using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

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
public class TEST_EAD_PoisonState : EntityAffector
{
    float mDurationTime;
    int mDamageAmount;
    Material mSkin;
    ParticleSystem mParticle;
    ///////////////////////////////////////////
    PlayerAction player;
    PlayerVisualData playerVisualData;

    ///////////////////////////////////////////
    Sandbag sandbag;
    ///////////////////////////////////////////
    
    List<UnityAction> Modifier;

    public TEST_EAD_PoisonState(GameObject _owner, GameObject _target, object[] _params) : base(_owner, _target, _params)
    {
        //Target은 의미 없을것이다.
        if (this.Params == null) { Debug.LogError("0: 유지시간 1:도트뎀 을 적어서 보내야함"); }

        mDurationTime = (float)Params[0];
        mDamageAmount = (int)Params[1];
        mSkin = (Material)Params[2];
        mParticle = (ParticleSystem)Params[3];

        Modifier = new List<UnityAction>();

        this.AsyncAffectorCoroutine.Add(DotDamage());
        
        if(_target.TryGetComponent<PlayerAction>(out PlayerAction playerData)){
            _target.TryGetComponent<PlayerVisualData>(out playerVisualData);
            Modifier.Add(Player_DotDamageModifier);
            this.AsyncAffectorCoroutine.Add(PlayerVisualActivate());
        }
        else if (_target.TryGetComponent<Sandbag>(out Sandbag sandbag)){
            Modifier.Add(SandBag_DotDamageModifier);
        }

    }
    public override void Affect()
    {
        this.Target.GetComponent<IAffectableEntity>().AsyncAffectHandler(this.AsyncAffectorCoroutine);
    }
    IEnumerator DotDamage()
    {
        float passedTime = 0;
        while (mDurationTime > passedTime)
        {
            passedTime += 0.5f;
            Modifier[0].Invoke();
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }

    void Player_DotDamageModifier(){player.GetDamaged(mDamageAmount);}
    void SandBag_DotDamageModifier(){sandbag.GetDamaged(mDamageAmount);}

    IEnumerator PlayerVisualActivate()
    {
        playerVisualData.skinModulator.SetSkinSets(1, mSkin);
        playerVisualData.particleModulator.ActivateParticle(mParticle, mDurationTime);
        yield return YieldInstructionCache.WaitForSeconds(mDurationTime);
        playerVisualData.skinModulator.SetSkinSets(1, null);
    }
}