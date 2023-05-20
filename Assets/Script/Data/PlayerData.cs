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
    public int MaxStamina {get; set;}
    public int CurStamina {get; set;}
    public float Power {get; set;}
    public int Luck {get; set;}
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

    public PlayerData(ScriptableObjPlayerData _playerScriptable) : base(_playerScriptable) {
        this.MaxStamina = _playerScriptable.maxStamina;
        this.CurStamina = this.MaxStamina;
        this.Power = _playerScriptable.power;
        this.Luck = _playerScriptable.luck;
        this.Gear = _playerScriptable.gear;
        this.Frag = _playerScriptable.frag;
        SkillState = () => {};
        InteractState = () => {};
        UpdateState = () => {};
    }
}