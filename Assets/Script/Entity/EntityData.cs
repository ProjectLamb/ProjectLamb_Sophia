using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.
public abstract class EntityData {
    public EntityData(ScriptableObjEntityData _entityScriptable) {
        mMaxHP      = _entityScriptable.maxHP;
        mCurHP      = mMaxHP;
        mMoveSpace  = _entityScriptable.moveSpace;
        mDefence    = _entityScriptable.defence;
        mTenacity   = _entityScriptable.tenacity;
        MoveState  = () => {};
        AttackState= () => {};
        HitState   = () => {};
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState   = () => {};
    }
    protected int mMaxHP;
    protected int mCurHP;
    protected float mMoveSpace;
    protected float mDefence;
    protected float mTenacity;
    public UnityAction MoveState;
    public UnityAction AttackState;
    public UnityAction HitState;
    public UnityAction PhyiscTriggerState;// 콜라이더 닿으면
    public UnityAction DieState;
}