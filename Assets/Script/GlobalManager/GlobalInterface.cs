using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public delegate void UnityActionRef<T>(ref T input);

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
public interface IDieable
{
    public void Die();
}

public interface IModifier
{
    public void Modifiy();
}

/*
데이터 변환이 이루어 진다.
*/
public interface IAffectable
{
    public void AffectHandler(AffectorPackage affectorPackage);
}

public interface IEntityDataAddressable
{
    public ref EntityData GetFinalData();
    public EntityData GetOriginData();
    public void ResetData();
}

public interface ITimeAffectable
{
    public void Pause();
    public void Play();
}

public interface IPurchase {
    
}