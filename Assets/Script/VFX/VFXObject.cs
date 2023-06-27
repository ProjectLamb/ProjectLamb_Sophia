using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 게임에 영향을 일절 주지 않고, 오직 비주얼적인 역할만 담당하는 컴포넌트 주로 ParticleSystem에 붙음 <br/>
/// > Method : 파괴되는 타이밍
/// </summary>
public class VFXObject : MonoBehaviour
{
    [Tooltip("IsNoneStacking = true면 단 하나만 생성되고 끝날때 까지는 생성할 수 없음을 나타냄")]
    public BUCKET_STACKING_TYPE         BucketStaking;
    public float                        DurateTime;
    public bool                         IsLooping;
    public STATE_TYPE                   AffectorType;
    public ParticleSystem               Particle;
    public UnityEvent                   OnDestroyEvent;
    public UnityAction<STATE_TYPE>      OnDestroyActionByState;
    private ParticleSystem.MainModule   mParticleModule;
    //단, 이 오브젝트는 무조건 파티클의 부모, 자식으로 구성된 놈만.
    private void Awake()
    {
        Particle ??= GetComponent<ParticleSystem>();
        mParticleModule = Particle.main;
        DurateTime = mParticleModule.duration + 0.01f;
    }
    public void SetScale(float _sizeRatio){transform.localScale *= _sizeRatio;}

    public void DestroyVFX()
    {
        Destroy(gameObject, this.DurateTime);
    }
    public void DestroyVFXForce()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() { 
        OnDestroyEvent?.Invoke(); 
        OnDestroyActionByState?.Invoke(AffectorType);
        OnDestroyActionByState = null;
    }
}
