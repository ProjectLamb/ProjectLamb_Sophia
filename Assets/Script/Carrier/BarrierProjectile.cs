using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Instantiate().Initialize(int amount, Entity owner)를 꼭 설정해야함 <br/>
/// 초기화가 잘 되었는지 예외처리도 하겠다. 
/// 프로젝타일은 다~~ 오브젝트 풀 사용하게끔 해보자.
/// </summary>
public class BarrierProjectile : Projectile
{
    public BarrierState barrierState;

    protected override void Awake()
    {
        base.Awake();
        this.projectileAffector.Add(barrierState);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Entity>(out Entity targetEntity))
        {
            barrierState.Init(targetEntity, targetEntity).Modifiy();
        }
    }
}