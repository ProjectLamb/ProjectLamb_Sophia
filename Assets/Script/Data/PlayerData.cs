using UnityEngine;
using UnityEngine.Events;


// 엔티티를 구성하는 가장 Atomic한 데이터
// 변동하는 데이터가 되는 녀석들이다.
[System.Serializable]
public class PlayerData : EntityData {
//  public int MaxHP {get {return mMaxHP;} set{mMaxHP = value;}}
//  public int CurHP {get {return mCurHP;} set{mCurHP = value;}}
//  public float MoveSpeed {get {return mMoveSpeed;} set{mMoveSpeed = value;}}
//  public float Defence {get {return mDefence;} set{mDefence = value;}}
//  public float Tenacity {get {return mTenacity;} set{mTenacity = value;}}
    [field : SerializeField] public int MaxStamina {get; set;}
    [field : SerializeField] public int CurStamina {get; set;}
    [field : SerializeField] public float StaminaRestoreRatio {get; set;}
    [field : SerializeField] public int Luck {get; set;}
    [field : SerializeField] public int Gear {get; set;}
    [field : SerializeField] public int Frag {get; set;}
//  public UnityAction mMoveState;
//  public UnityAction mAttackState;
//  public UnityAction mHitState;
//  public UnityAction mPhyiscTriggerState;// 콜라이더 닿으면
//  public UnityAction mDieState;
    public UnityAction SkillState;
    public UnityAction InteractState;
    public UnityAction UpdateState;

    public PlayerData() : base() {
        SkillState = () => {};
        InteractState = () => {};
        UpdateState = () => {};
    }
}