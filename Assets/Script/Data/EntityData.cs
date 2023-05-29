using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.

// 모든것은 올라가면 효과가 좋아지는 방식이다.

public abstract class EntityData {
    [field : SerializeField] string mEntityTag;
    public string EntityTag {get {return mEntityTag;} set {mEntityTag = value;}}
    [field : SerializeField] int mMaxHP;
    public int MaxHP {get {return mMaxHP;} set{mMaxHP = value;}}
    [field : SerializeField] int mCurHP;
    public int CurHP {get {return mCurHP;} set{mCurHP = value;}}
    [field : SerializeField] float mMoveSpeed;
    public float MoveSpeed {get {return mMoveSpeed;} set{mMoveSpeed = value;}}
    [field : SerializeField] float mDefence;
    public float Defence {get {return mDefence;} set{mDefence = value;}}
    [field : SerializeField] float mTenacity;
    public float Tenacity {get {return mTenacity;} set{mTenacity = value;}}
    [field : SerializeField] int mPower;
    public int Power {get {return mPower;} set{mPower = value;}}
    [field : SerializeField] float mAttackSpeed; 
    public float AttackSpeed {get {return mAttackSpeed;} set{mAttackSpeed = value;}}
       
    public UnityAction MoveState = () => {};
    public UnityAction AttackState = () => {};     //몬스터 패턴 각각에 어텍 스테이트를 만들어야 할지도 있다.
    public UnityActionRef<int> AttackStateRef = (ref int i) => {};
    public UnityAction HitState        = () => {};
    public UnityActionRef<int> HitStateRef     = (ref int i) => {};
    public UnityAction<Entity, Entity> ProjectileShootState = (owner, target) => {};
    public UnityAction PhyiscTriggerState = () => {};// 콜라이더 닿으면
    public UnityAction DieState = () => {};
    
    public UnityAction UIAffectState = () => {};
    
    public EntityData() {
        MaxHP           = 0;
        CurHP           = 0;
        MoveSpeed       = 0f;
        Defence         = 0f;
        Tenacity        = 0f;
        Power           = 0;
        AttackSpeed     = 0f;
    }
}