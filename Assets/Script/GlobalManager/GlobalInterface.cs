using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 상호작용에 대한 인터페이스
/// </summary>
public interface IInteractable {
    public void Interact();
}

/// <summary>
/// 맞았을떄 대한 인터페이스 
/// 근데 이 GetDamage의 회의감?
/// </summary>
public interface IDamagable
{
   public void GetDamaged(int _amount);
   public void GetDamaged(int _amount, VFXObject _obj);
}

/// <summary>
/// 죽는것에 대한 Action
/// </summary>
public interface IDieAble
{
    public void Die();
}

public interface IModifier {
    public void Modifiy();
}

/*
데이터 변환이 이루어 진다.
*/
public interface IAffectable {
    public void AffectHandler(AffectorStruct affectorStruct);
}

public interface IEntityAddressable : IDamagable, IDieAble, IAffectable, IEntityDataAddressable { }

public interface IEntityDataAddressable {
    public ref  EntityData GetFinalData();
    public      EntityData GetOriginData();
    public void ResetData();
}

public interface IVisuallyInteractable {
    public void InteractByMaterial(Material material, float dutateTime);
    public void InteractByParticle(ParticleSystem particleSystem, float dutateTime);
    public void Revert();
} 

public interface IColliderHandleable{
    public void HandleCollider(){}
}

public interface ISetByObjectable{
    public void SetByObject(float[] objects);
}

public interface IDestroyHandler {
    public void DestroySelf(UnityAction _callback);
    public void DestroySelf(UnityAction _callback, float _time);
}