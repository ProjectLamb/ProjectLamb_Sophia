using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.

// 모든것은 올라가면 효과가 좋아지는 방식이다.
[System.Serializable]
public struct EntityData
{
    //int CurHP; 이 EntityData를 포함하는 컴포넌트그 스코프 내에서 따로 정의한다. 오직 이 데이터
    public string EntityTag;
    public int MaxHP;
    public int Power;
    public float MoveSpeed;
    public float Defence;
    public float Tenacity;
    public float AttackSpeed;
    public UnityAction MoveState;
    public UnityAction AttackState;
    public UnityActionRef<int> AttackStateRef;
    public UnityAction HitState;
    public UnityActionRef<int> HitStateRef;
    public UnityAction<Entity, Entity> ProjectileShootState;
    public UnityAction PhyiscTriggerState;
    public UnityAction DieState;
    public UnityAction UIAffectState;
    public EntityData(ScriptableObjEntityData _scriptable)
    {
        EntityTag = _scriptable.EntityTag;
        MaxHP = _scriptable.MaxHP;
        Power = _scriptable.Power;
        MoveSpeed = _scriptable.MoveSpeed;
        Defence = _scriptable.Defence;
        Tenacity = _scriptable.Tenacity;
        AttackSpeed = _scriptable.AttackSpeed;

        MoveState = _scriptable.MoveState;
        AttackState = _scriptable.AttackState;
        AttackStateRef = _scriptable.AttackStateRef;
        HitState = _scriptable.HitState;
        HitStateRef = _scriptable.HitStateRef;
        ProjectileShootState = _scriptable.ProjectileShootState;
        PhyiscTriggerState = _scriptable.PhyiscTriggerState;
        DieState = _scriptable.DieState;
        UIAffectState = _scriptable.UIAffectState;
    }
    public static EntityData operator +(EntityData x, EntityData y)
    {
        EntityData res = new EntityData();
        res.MaxHP = x.MaxHP + y.MaxHP;
        res.MoveSpeed = x.MoveSpeed + y.MoveSpeed;
        res.Defence = x.Defence + y.Defence;
        res.Tenacity = x.Tenacity + y.Tenacity;
        res.Power = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;

        res.MoveState = x.MoveState + y.MoveState;
        res.AttackState = x.AttackState + y.AttackState;
        res.AttackStateRef = x.AttackStateRef + y.AttackStateRef;
        res.HitState = x.HitState + y.HitState;
        res.HitStateRef = x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState = x.DieState + y.DieState;
        res.UIAffectState = x.UIAffectState + y.UIAffectState;
        return res;
    }
    public static EntityData operator -(EntityData x, EntityData y)
    {
        EntityData res = new EntityData();
        res.MaxHP = x.MaxHP - y.MaxHP;
        res.MoveSpeed = x.MoveSpeed - y.MoveSpeed;
        res.Defence = x.Defence - y.Defence;
        res.Tenacity = x.Tenacity - y.Tenacity;
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;

        res.MoveState = x.MoveState - y.MoveState;
        res.AttackState = x.AttackState - y.AttackState;
        res.AttackStateRef = x.AttackStateRef - y.AttackStateRef;
        res.HitState = x.HitState - y.HitState;
        res.HitStateRef = x.HitStateRef - y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState - y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState - y.PhyiscTriggerState;
        res.DieState = x.DieState - y.DieState;
        res.UIAffectState = x.UIAffectState - y.UIAffectState;
        return res;
    }
    public readonly override string ToString() => $" EntityTag : {EntityTag}, MaxHP : {MaxHP}, Power : {Power}, MoveSpeed : {MoveSpeed}, Defence : {Defence}, Tenacity : {Tenacity}, AttackSpeed : {AttackSpeed}";
}