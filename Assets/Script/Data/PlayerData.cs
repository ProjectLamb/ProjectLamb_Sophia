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
    public UnityAction SkillState = () => {};
    public UnityAction InteractState = () => {};
    public UnityAction UpdateState = () => {};

    public PlayerData() : base() {
        this.MaxStamina  = 0;
        this.CurStamina  = 0;
        this.StaminaRestoreRatio = 0f;
        this.Luck        = 0;
        this.Gear        = 0;
        this.Frag        = 0;
    }

    public PlayerData Clone(){
        PlayerData res = new PlayerData();
        res.EntityTag = this.EntityTag;
        res.MaxHP = this.MaxHP;
        res.CurHP = this.CurHP;
        res.MoveSpeed = this.MoveSpeed;
        res.Defence = this.Defence;
        res.Tenacity = this.Tenacity;
        res.Power = this.Power;
        res.AttackSpeed = this.AttackSpeed;
        
        res.MoveState       = this.MoveState;
        res.AttackState     = this.AttackState;
        res.AttackStateRef  = this.AttackStateRef;
        res.HitState        = this.HitState;
        res.HitStateRef     = this.HitStateRef;
        res.ProjectileShootState = this.ProjectileShootState;
        res.PhyiscTriggerState = this.PhyiscTriggerState;
        res.DieState        = this.DieState;
        res.UIAffectState   = this.UIAffectState;
        
        res.MaxStamina = this.MaxStamina;
        res.CurStamina = this.CurStamina;
        res.StaminaRestoreRatio = this.StaminaRestoreRatio;
        res.Luck = this.Luck;
        res.Gear = this.Gear;
        res.Frag = this.Frag;

        res.SkillState      = this.SkillState;
        res.InteractState   = this.InteractState;
        res.UpdateState     = this.UpdateState;

        return res;
    }

    public static PlayerData operator +(PlayerData x, MasterData y){
        PlayerData res = new PlayerData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;        
        res.Power = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        
        res.MoveState       = x.MoveState + y.MoveState;
        res.AttackState     = x.AttackState + y.AttackState;
        res.AttackStateRef  = x.AttackStateRef + y.AttackStateRef;
        res.HitState        = x.HitState + y.HitState;
        res.HitStateRef     = x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState        = x.DieState + y.DieState;
        res.UIAffectState   = x.UIAffectState + y.UIAffectState;

        res.MaxStamina = x.MaxStamina + y.MaxStamina;
        res.CurStamina = x.CurStamina + y.CurStamina;
        res.StaminaRestoreRatio = x.StaminaRestoreRatio + y.StaminaRestoreRatio;
        res.Luck = x.Luck + y.Luck;
        res.Gear = x.Gear + y.Gear;
        res.Frag = x.Frag + y.Frag;

        res.SkillState      = x.SkillState + y.SkillState;
        res.InteractState   = x.InteractState + y.InteractState;
        res.UpdateState     = x.UpdateState + y.UpdateState;

        return res;
    }
    public static PlayerData operator -(PlayerData x, MasterData y){
        PlayerData res = new PlayerData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;        
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;
        
        res.MoveState       = x.MoveState - y.MoveState;
        res.AttackState     = x.AttackState - y.AttackState;
        res.AttackStateRef  = x.AttackStateRef - y.AttackStateRef;
        res.HitState        = x.HitState - y.HitState;
        res.HitStateRef     = x.HitStateRef - y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState - y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState - y.PhyiscTriggerState;
        res.DieState        = x.DieState - y.DieState;
        res.UIAffectState   = x.UIAffectState - y.UIAffectState;

        res.MaxStamina = x.MaxStamina - y.MaxStamina;
        res.CurStamina = x.CurStamina - y.CurStamina;
        res.StaminaRestoreRatio = x.StaminaRestoreRatio - y.StaminaRestoreRatio;
        res.Luck = x.Luck - y.Luck;
        res.Gear = x.Gear - y.Gear;
        res.Frag = x.Frag - y.Frag;

        res.SkillState      = x.SkillState - y.SkillState;
        res.InteractState   = x.InteractState - y.InteractState;
        res.UpdateState     = x.UpdateState - y.UpdateState;
        return res;
    }

}