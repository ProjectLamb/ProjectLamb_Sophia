using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.
public class PlayerEntityData : EntityData {
    public PlayerEntityData(ScriptableObjPlayerData _playerScriptable) : base(_playerScriptable) {
        this.Power = _playerScriptable.power;
        this.Luck = _playerScriptable.luck;
        this.Gear = _playerScriptable.gear;
        this.Frag = _playerScriptable.frag;
        SkillState = () => {};
        InteractState = () => {};
        UpdateState = () => {};
    }
    public int MaxHp {get {return this.mMaxHP;} set{mMaxHP = value;}}
    public int CurHp {get {return mMaxHP;} set{mMaxHP = value;}}
    public float MoveSpace {get {return mMoveSpace;} set{mMoveSpace = value;}}
    public float Defence {get {return mDefence;} set{mDefence = value;}}
    public float Tenacity {get {return mTenacity;} set{mTenacity = value;}}
    public float Power {get; set;}
    public float Luck {get; set;}
    public int Gear {get; set;}
    public int Frag {get; set;}
//  public UnityAction mMoveState;
//  public UnityAction mAttackState;
//  public UnityAction mHitState;
//  public UnityAction mPhyiscTriggerState;// 콜라이더 닿으면
//  public UnityAction mDieState;
    public UnityAction SkillState;
    public UnityAction InteractState;
    public UnityAction UpdateState;
}