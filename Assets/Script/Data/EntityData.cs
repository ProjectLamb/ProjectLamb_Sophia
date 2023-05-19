using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.

public abstract class EntityData {
    int mMaxHP;
        public int MaxHP {get {return mMaxHP;} set{mMaxHP = value;}}
    int mCurHP;
        public int CurHP {get {return mCurHP;} set{mCurHP = value;}}
    float mMoveSpeed;
        public float MoveSpeed {get {return mMoveSpeed;} set{mMoveSpeed = value;}}
    float mDefence;
        public float Defence {get {return mDefence;} set{mDefence = value;}}
    float mTenacity;
        public float Tenacity {get {return mTenacity;} set{mTenacity = value;}}
    
    public ParticleSystem DieParticle;
    public UnityAction MoveState;
    public UnityAction AttackState;     //몬스터 패턴 각각에 어텍 스테이트를 만들어야 할지도 있다.
    public UnityAction HitState;
    public UnityAction PhyiscTriggerState;// 콜라이더 닿으면
    public UnityAction DieState;
    
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
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState        = () => {};
    }
}