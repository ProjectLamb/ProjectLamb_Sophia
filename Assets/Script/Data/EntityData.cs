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
    public UnityActionRef<int> AttackStateRef;
    public UnityAction HitState;
    public UnityActionRef<int> HitStateRef;
    public UnityAction PhyiscTriggerState;// 콜라이더 닿으면
    public UnityAction DieState;
    public UnityAction<GameObject> ProjectileShootState;
    
    public UnityAction UIAffectState;
    
    public EntityData() {
        this.MaxHP       = 0;
        this.CurHP       = 0;
        this.MoveSpeed   = 0f;
        this.Defence     = 0f;
        this.Tenacity    = 0f;
        this.Power       = 0;
        this.AttackSpeed = 0f;

        MoveState       = () => {};
        AttackState     = () => {}; 
        AttackStateRef     = (ref int i) => {};
        HitState        = () => {};
        HitStateRef     = (ref int i) => {};
        ProjectileShootState = (obj) => {};
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState        = () => {};
        UIAffectState   = () => {};
    }
    // Abstract 는 new 할 수 없다.
    //public static EntityData operator +(EntityData x, MasterData y) {
    //    EntityData res = new EntityData();
    //    return result;
    //}
}