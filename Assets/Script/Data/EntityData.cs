using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.

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

    [field : SerializeField] public ParticleSystem DieParticle;
    public UnityAction MoveState;
    public UnityAction AttackState;     //몬스터 패턴 각각에 어텍 스테이트를 만들어야 할지도 있다.
    public UnityAction HitState;
    public UnityAction PhyiscTriggerState;// 콜라이더 닿으면
    public UnityAction DieState;
    public UnityAction<GameObject> ProjectileShootState;
    public UnityActionRef<int> HitStateRef;
    public UnityAction UIAffectState;
    
    public EntityData(ScriptableObjEntityData _entityScriptable) {
        MaxHP           = _entityScriptable.maxHP;
        CurHP           = MaxHP;
        MoveSpeed       = _entityScriptable.moveSpeed;
        Defence         = _entityScriptable.defence;
        Tenacity        = _entityScriptable.tenacity;
        DieParticle     = _entityScriptable.dieParticle;
        MoveState       = () => {};
        AttackState     = () => {}; 
        HitState        = () => {};
        HitStateRef     = (ref int i) => {};
        ProjectileShootState = (obj) => {};
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState        = () => {};
        UIAffectState   = () => {};
    }
}