using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//모든 합연산같은 경우 다 파이프라인으로 진행한다.

/// <summary>
/// 특징 : 데이터 전달을 하는데 사용한다. <br/>
/// Adding과 다르게 마스터는 그 대로 있어도 괜찮다. 이 값이 최종 값이된다   <br/>
/// 특징 : 파이프라인은 어떤 데이터와도 치환 가능하다.  <br/>
///     >  MasterData.PipeToEntity(ref EntityData _data); <br/>
///     >  MasterData.PipeToPlayer(ref PlayerData _data); <br/>
///     >  MasterData.PipeToSkill(ref SkillData _data); <br/>
///     >  MasterData.PipeToWeapon(ref WeaponData _data); <br/>
/// 디버깅 : 모든 멤버를 ToString화 할 수 있다.
///     > pipeline.ToString();
/// </summary>
[System.Serializable]
public class MasterData {    
    [field : SerializeField] public int      MaxHP       {get; set;}
    [field : SerializeField] public int      CurHP       {get; set;}
    [field : SerializeField] public float    MoveSpeed   {get; set;}
    [field : SerializeField] public float    Defence     {get; set;}
    [field : SerializeField] public float    Tenacity    {get; set;}
    [field : SerializeField] public int      MaxStamina  {get; set;}
    [field : SerializeField] public int      CurStamina  {get; set;}
    [field : SerializeField] public float    StaminaRestoreRatio  {get; set;}
    [field : SerializeField] public int      Power       {get; set;}
    [field : SerializeField] public float    AttackSpeed {get; set;}
    [field : SerializeField] public int      Luck        {get; set;}
    [field : SerializeField] public int      Gear        {get; set;}
    [field : SerializeField] public int      Frag        {get; set;}
    [field : SerializeField] public float    DamageRatio {get; set;}
    [field : SerializeField] public float    WeaponDelay {get; set;}
    [field : SerializeField] public float    Range       {get; set;}
    //[field : SerializeField] public int      Ammo        {get; set;}
    public SkillRankInfo[] SkillRankInfos;

    //////////////////////////////////////////////////////
    public UnityAction MoveState       = () => {};
    public UnityAction AttackState     = () => {}; 
    public UnityActionRef<int> AttackStateRef  = (ref int i) => {};
    public UnityAction HitState        = () => {};
    public UnityActionRef<int> HitStateRef     = (ref int i) => {};
    public UnityAction<Entity, Entity> ProjectileShootState = (_owner, _target) => {};
    public UnityAction PhyiscTriggerState = () => {};
    public UnityAction DieState        = () => {};
    public UnityAction UIAffectState   = () => {};
    //////////////////////////////////////////////////////
    public UnityAction SkillState = () => {};
    public UnityAction InteractState = () => {};
    public UnityAction UpdateState = () => {};
    
    //////////////////////////////////////////////////////
    public UnityAction WeaponUseState = () => {};
    public UnityAction WeaponChangeState = () => {};
    public UnityAction WeaponReLoadState = () => {};
    //////////////////////////////////////////////////////
    
    //Index는 E_SkillKey
    public UnityAction SkillUseState = () => {};
    public UnityAction SkillLevelUpState = () => {};
    public UnityAction SkillChangeState = () => {};
    
    public MasterData(){
        MaxHP       = 0;
        CurHP       = 0;
        MoveSpeed   = 0f;
        Defence     = 0f;
        Tenacity    = 0f;
        MaxStamina  = 0;
        CurStamina  = 0;
        StaminaRestoreRatio = 0f;
        Power       = 0;
        AttackSpeed = 0f;
        Luck        = 0;
        Gear        = 0;
        Frag        = 0;
        DamageRatio = 0f;
        WeaponDelay = 0f;
        Range       = 0f;
        //Ammo        = 0;
        SkillRankInfos = new SkillRankInfo[3];
        for(int i = 0; i < 3; i++){SkillRankInfos[i] = new SkillRankInfo();}        

    }
    
    public void Clear(){
        MaxHP       = 0;
        CurHP       = 0;
        MoveSpeed   = 0f;
        Defence     = 0f;
        Tenacity    = 0f;
        MaxStamina  = 0;
        CurStamina  = 0;
        StaminaRestoreRatio = 0f;
        Power       = 0;
        AttackSpeed = 0f;
        Luck        = 0;
        Gear        = 0;
        Frag        = 0;
        DamageRatio = 0f;
        WeaponDelay = 0f;
        Range       = 0f;
        //Ammo        = 0;
        for(int i = 0; i < 3; i++){SkillRankInfos[i].Clear();}

        MoveState       = () => {};
        AttackState     = () => {}; 
        AttackStateRef     = (ref int i) => {};
        HitState        = () => {};
        HitStateRef     = (ref int i) => {};
        ProjectileShootState = (_owner, _target) => {};
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState        = () => {};
        UIAffectState   = () => {};

        SkillState = () => {};
        InteractState = () => {};
        UpdateState = () => {};
        
        WeaponUseState = () => {};
        WeaponChangeState = () => {};
        WeaponReLoadState = () => {};

        SkillUseState = () => {};
        SkillLevelUpState = () => {};
        SkillChangeState = () => {};
    }

    public MasterData Clone(){
        MasterData res = new MasterData();
        res.MaxHP       = this.MaxHP;
        res.CurHP       = this.CurHP;
        res.MoveSpeed   = this.MoveSpeed;
        res.Defence     = this.Defence;
        res.Tenacity    = this.Tenacity;
        res.MaxStamina  = this.MaxStamina;
        res.CurStamina  = this.CurStamina;
        res.StaminaRestoreRatio  = this.StaminaRestoreRatio;
        res.Power       = this.Power;
        res.AttackSpeed = this.AttackSpeed;
        res.Luck        = this.Luck;
        res.Gear        = this.Gear;
        res.Frag        = this.Frag;
        res.DamageRatio = this.DamageRatio;
        res.WeaponDelay = this.WeaponDelay;
        res.Range       = this.Range;
        //res.Ammo        = x.Ammo        + y.Ammo;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillRankInfos[i].numericArray[j]   = this.SkillRankInfos[i].numericArray[j];
                res.SkillRankInfos[i].skillDelay[j]     = this.SkillRankInfos[i].skillDelay[j];
                res.SkillRankInfos[i].durateTime[j]     = this.SkillRankInfos[i].durateTime[j];
            }
        }

        res.MoveState       = this.MoveState;
        res.AttackState     = this.AttackState;
        res.AttackStateRef     = this.AttackStateRef;
        res.HitState        = this.HitState;
        res.HitStateRef     = this.HitStateRef;
        res.ProjectileShootState = this.ProjectileShootState;
        res.PhyiscTriggerState = this.PhyiscTriggerState;
        res.DieState        = this.DieState;
        res.UIAffectState   = this.UIAffectState;
        res.SkillState      = this.SkillState;
        res.InteractState   = this.InteractState;
        res.UpdateState     = this.UpdateState;
        res.WeaponUseState = this.WeaponUseState;
        res.WeaponChangeState = this.WeaponChangeState;
        res.WeaponReLoadState = this.WeaponReLoadState;
        res.SkillUseState = this.SkillUseState;
        res.SkillLevelUpState = this.SkillLevelUpState;
        res.SkillChangeState = this.SkillChangeState;
        return res;
    }
    public static MasterData operator +(MasterData x, MasterData y){
        MasterData res = new MasterData();
        res.MaxHP       = x.MaxHP       + y.MaxHP;
        res.CurHP       = x.CurHP       + y.CurHP;
        res.MoveSpeed   = x.MoveSpeed   + y.MoveSpeed;
        res.Defence     = x.Defence     + y.Defence;
        res.Tenacity    = x.Tenacity    + y.Tenacity;
        res.MaxStamina  = x.MaxStamina  + y.MaxStamina;
        res.CurStamina  = x.CurStamina  + y.CurStamina;
        res.StaminaRestoreRatio  = x.StaminaRestoreRatio  + y.StaminaRestoreRatio;
        res.Power       = x.Power       + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        res.Luck        = x.Luck        + y.Luck;
        res.Gear        = x.Gear        + y.Gear;
        res.Frag        = x.Frag        + y.Frag;
        res.DamageRatio = x.DamageRatio + y.DamageRatio;
        res.WeaponDelay = x.WeaponDelay + y.WeaponDelay;
        res.Range       = x.Range       + y.Range;
        //res.Ammo        = x.Ammo        + y.Ammo;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillRankInfos[i].numericArray[j]   = x.SkillRankInfos[i].numericArray[j] + y.SkillRankInfos[i].numericArray[j];
                res.SkillRankInfos[i].skillDelay[j]     = x.SkillRankInfos[i].skillDelay[j] + y.SkillRankInfos[i].skillDelay[j];
                res.SkillRankInfos[i].durateTime[j]     = x.SkillRankInfos[i].durateTime[j] + y.SkillRankInfos[i].durateTime[j];
            }
        }

        res.MoveState       = x.MoveState + y.MoveState;
        res.AttackState     = x.AttackState + y.AttackState;
        res.AttackStateRef     = x.AttackStateRef + y.AttackStateRef;
        res.HitState        = x.HitState + y.HitState;
        res.HitStateRef     = x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState = x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState = x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState        = x.DieState + y.DieState;
        res.UIAffectState   = x.UIAffectState + y.UIAffectState;

        res.SkillState      = x.SkillState + y.SkillState;
        res.InteractState   = x.InteractState + y.InteractState;
        res.UpdateState     = x.UpdateState + y.UpdateState;

        res.WeaponUseState = x.WeaponUseState + y.WeaponUseState;
        res.WeaponChangeState = x.WeaponChangeState + y.WeaponChangeState;
        res.WeaponReLoadState = x.WeaponReLoadState + y.WeaponReLoadState;

        res.SkillUseState = x.SkillUseState + y.SkillUseState;
        res.SkillLevelUpState = x.SkillLevelUpState + y.SkillLevelUpState;
        res.SkillChangeState = x.SkillChangeState + y.SkillChangeState;
        return res;
    }

    public override string ToString(){
        string PlayerString = $"MaxHP : {MaxHP}, CurHP : {CurHP}, MoveSpeed : {MoveSpeed}, Defence : {Defence}, Tenacity : {Tenacity}, MaxStamina : {MaxStamina}, CurStamina : {CurStamina}, StaminaRestoreRatio : {StaminaRestoreRatio}, Power : {Power}, AttackSpeed : {AttackSpeed}, Luck : {Luck}, Gear : {Gear}, Frag : {Frag} \n";
        string WeaponString = $"DamageRatio : {DamageRatio}, WeaponDelay : {WeaponDelay}, Range : {Range}\n";
        //string WeaponString = $"DamageRatio : {DamageRatio}, WeaponDelay : {WeaponDelay}, Range : {Range}, Ammo : {Ammo}\n";
        string SkillString = "";
        for(int i = 0; i < 3; i++){
            switch (i){
                case 0 :
                    SkillString += "Q Key : ";
                    break;
                case 1 :
                    SkillString += "E Key : ";
                    break;
                case 2 :
                    SkillString += "R Key : ";
                    break;
            }
            for(int j = 0; j < 3; j++){
                SkillString += $"NumericArray : {SkillRankInfos[i].numericArray[j]}, ";
                SkillString += $"SkillDelay : {SkillRankInfos[i].skillDelay[j]}, ";
                SkillString += $"DurateTime : {SkillRankInfos[i].durateTime[j]}\n";
            }
        }
        return (PlayerString + WeaponString + SkillString);
    }

    public void PipeToEntity(ref EntityData _entity){
        _entity.MaxHP = this.MaxHP;
        _entity.CurHP = this.CurHP;
        _entity.MoveSpeed = this.MoveSpeed;
        _entity.Defence = this.Defence;
        _entity.Tenacity = this.Tenacity;
        _entity.Power = this.Power;
        _entity.AttackSpeed = this.AttackSpeed;

        _entity.MoveState = this.MoveState;
        _entity.AttackState = this.AttackState;
        _entity.AttackStateRef = this.AttackStateRef;
        _entity.HitState = this.HitState;
        _entity.HitStateRef = this.HitStateRef;
        _entity.ProjectileShootState = this.ProjectileShootState;
        _entity.PhyiscTriggerState = this.PhyiscTriggerState;
        _entity.DieState = this.DieState;
        _entity.UIAffectState = this.UIAffectState;
    }
    public void PipeToPlayer(ref PlayerData _player){
        _player.MaxHP   = this.MaxHP;
        _player.CurHP   = this.CurHP;
        _player.MoveSpeed   = this.MoveSpeed;
        _player.Defence = this.Defence;
        _player.Tenacity    = this.Tenacity;
        _player.MaxStamina  = this.MaxStamina;
        _player.CurStamina  = this.CurStamina;
        _player.StaminaRestoreRatio = this.StaminaRestoreRatio;
        _player.Power   = this.Power;
        _player.AttackSpeed = this.AttackSpeed;
        _player.Luck    = this.Luck;
        _player.Gear    = this.Gear;
        _player.Frag    = this.Frag;
        
        _player.MoveState = this.MoveState;
        _player.AttackState = this.AttackState;
        _player.AttackStateRef = this.AttackStateRef;
        _player.HitState = this.HitState;
        _player.HitStateRef = this.HitStateRef;
        _player.ProjectileShootState = this.ProjectileShootState;
        _player.PhyiscTriggerState = this.PhyiscTriggerState;
        _player.DieState = this.DieState;
        _player.UIAffectState = this.UIAffectState;
        _player.SkillState = this.SkillState;
        _player.InteractState = this.InteractState;
        _player.UpdateState = this.UpdateState;
    }
    public void PipeToWeapon(ref WeaponData _weapon){
        _weapon.DamageRatio     = this.DamageRatio;
        _weapon.WeaponDelay     = this.WeaponDelay;
        _weapon.Range           = this.Range;

        _weapon.WeaponUseState      =this.WeaponUseState;
        _weapon.WeaponChangeState   =this.WeaponChangeState;
        //if(_weapon.WeaponType == E_WeaponType.ranger){
        //    _weapon.Ammo= this.Ammo;
        //    _weapon.WeaponReLoadState   =this.WeaponReLoadState;
        //}
        //else {
        //    Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        //}
    }
    public void PipeToSkill(ref SkillData _skill){
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                _skill.SkillRankInfos[i].numericArray[j]   = this.SkillRankInfos[i].numericArray[j];
                _skill.SkillRankInfos[i].skillDelay[j]     = this.SkillRankInfos[i].skillDelay[j];
                _skill.SkillRankInfos[i].durateTime[j]     = this.SkillRankInfos[i].durateTime[j];
            }
        }
        _skill.SkillUseState = this.SkillUseState;
        _skill.SkillLevelUpState = this.SkillLevelUpState;
        _skill.SkillChangeState = this.SkillChangeState;
    }
    //  데미지 공식을 적을 수 있다.
}