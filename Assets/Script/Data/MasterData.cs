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
    [field : SerializeField] public int      Ammo        {get; set;}
    public SkillInfo[] SkillInfos;

    //////////////////////////////////////////////////////
    public UnityAction MoveState;
    public UnityAction AttackState;     //몬스터 패턴 각각에 어텍 스테이트를 만들어야 할지도 있다.
    public UnityAction HitState;
    public UnityAction PhyiscTriggerState;// 콜라이더 닿으면
    public UnityAction DieState;
    public UnityAction<GameObject> ProjectileShootState;
    public UnityActionRef<int> HitStateRef;
    public UnityAction UIAffectState;
    //////////////////////////////////////////////////////
    public UnityAction SkillState;
    public UnityAction InteractState;
    public UnityAction UpdateState;
    
    //////////////////////////////////////////////////////
    public UnityAction WeaponUseState;
    public UnityAction WeaponChangeState;
    public UnityAction WeaponReLoadState;
    //////////////////////////////////////////////////////
    
    //Index는 E_SkillKey
    public UnityAction[] SkillUseState;
    public UnityAction[] SkillChangeState;
    public UnityAction[] SkillLevelUpState;
    
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
        Ammo        = 0;
        SkillInfos = new SkillInfo[3];
        for(int i = 0; i < 3; i++){SkillInfos[i] = new SkillInfo();}

        MoveState       = () => {};
        AttackState     = () => {}; 
        HitState        = () => {};
        HitStateRef     = (ref int i) => {};
        ProjectileShootState = (obj) => {};
        PhyiscTriggerState = () => {};// 콜라이더 닿으면
        DieState        = () => {};
        UIAffectState   = () => {};

        SkillState = () => {};
        InteractState = () => {};
        UpdateState = () => {};
        
        WeaponUseState = () => {};
        WeaponChangeState = () => {};
        WeaponReLoadState = () => {};

        for(int i = 0; i < 3; i++){
            SkillUseState[i] = () => {};
            SkillChangeState[i] = () => {};
            SkillLevelUpState[i] = () => {};
        }
    }
    /*
    public static MasterData operator +(MasterData x, EntityData y)
    {
        MasterData res = new MasterData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.Power   = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        
        res.MoveState       =   x.MoveState + y.MoveState;
        res.AttackState     =   x.AttackState + y.AttackState;
        res.HitState        =   x.HitState + y.HitState;
        res.HitStateRef     =   x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState =  x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState =    x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState        =   x.DieState + y.DieState;
        res.UIAffectState   =   x.UIAffectState + y.UIAffectState;
        return res;
    }
    public static MasterData operator +(MasterData x, PlayerData y)
    {
        MasterData res = new MasterData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.Power   = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;

        res.MaxStamina = x.MaxStamina +y.MaxStamina;
        res.CurStamina = x.CurStamina +y.CurStamina;
        res.StaminaRestoreRatio = x.StaminaRestoreRatio + y.StaminaRestoreRatio;
        res.Luck = x.Luck +y.Luck;
        res.Gear = x.Gear +y.Gear;
        res.Frag = x.Frag +y.Frag;

        res.MoveState       =   x.MoveState + y.MoveState;
        res.AttackState     =   x.AttackState + y.AttackState;
        res.HitState        =   x.HitState + y.HitState;
        res.HitStateRef     =   x.HitStateRef + y.HitStateRef;
        res.ProjectileShootState =  x.ProjectileShootState + y.ProjectileShootState;
        res.PhyiscTriggerState =    x.PhyiscTriggerState + y.PhyiscTriggerState;
        res.DieState        =   x.DieState + y.DieState;
        res.UIAffectState   =   x.UIAffectState + y.UIAffectState;

        res.SkillState = x.SkillState + y.SkillState;
        res.InteractState = x.InteractState + y.InteractState;
        res.UpdateState = x.UpdateState + y.UpdateState;
        return res;
    }
    public static MasterData operator +(MasterData x, WeaponData y)
    {
        MasterData res = new MasterData();
        res.DamageRatio     = x.DamageRatio + y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay + y.WeaponDelay;
        res.Range           = x.Range + y.Range;
        
        res.WeaponUseState = x.WeaponUseState + y.WeaponUseState;
        res.WeaponChangeState = x.WeaponChangeState + y.WeaponChangeState;

        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo + y.Ammo;
            res.WeaponReLoadState = x.WeaponReLoadState + y.WeaponReLoadState;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }

        
        return res;
    }
    public static MasterData operator +(MasterData x, SkillData y){
        MasterData res = new MasterData();

        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] + y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] + y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] + y.SkillInfos[i].durateTime[j];
            }
        }
        res.SkillUseState[(int)y.CurrentSkillKey] = y.SkillUseState;
        res.SkillChangeState[(int)y.CurrentSkillKey] = y.SkillChangeState;
        res.SkillLevelUpState[(int)y.CurrentSkillKey] = y.SkillLevelUpState;
        return res;
    }
    public static MasterData operator +(EntityData x, MasterData y) { return y + x; }
    public static MasterData operator +(PlayerData y, MasterData x) { return y + x; }
    public static MasterData operator +(WeaponData y, MasterData x) { return y + x; }
    public static MasterData operator +(SkillData y, MasterData x)  { return y + x; }
    */
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
        res.Ammo        = x.Ammo        + y.Ammo;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] + y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] + y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] + y.SkillInfos[i].durateTime[j];
            }
        }

        res.MoveState       = x.MoveState + y.MoveState;
        res.AttackState     = x.AttackState + y.AttackState;
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

        for(int i = 0; i < 3; i++){
            res.SkillUseState[i] = x.SkillUseState[i] + y.SkillUseState[i];
            res.SkillChangeState[i] = x.SkillChangeState[i] + y.SkillChangeState[i];
            res.SkillLevelUpState[i] = x.SkillLevelUpState[i] + y.SkillLevelUpState[i];
        }
        return res;
    }

    public override string ToString(){
        string PlayerString = $"MaxHP : {MaxHP}, CurHP : {CurHP}, MoveSpeed : {MoveSpeed}, Defence : {Defence}, Tenacity : {Tenacity}, MaxStamina : {MaxStamina}, CurStamina : {CurStamina}, StaminaRestoreRatio : {StaminaRestoreRatio}, Power : {Power}, AttackSpeed : {AttackSpeed}, Luck : {Luck}, Gear : {Gear}, Frag : {Frag} \n";
        string WeaponString = $"DamageRatio : {DamageRatio}, WeaponDelay : {WeaponDelay}, Range : {Range}, Ammo : {Ammo}\n";
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
                SkillString += $"NumericArray : {SkillInfos[i].numericArray[j]}, ";
                SkillString += $"SkillDelay : {SkillInfos[i].skillDelay[j]}, ";
                SkillString += $"DurateTime : {SkillInfos[i].durateTime[j]}\n";
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
        if(_weapon.WeaponType == E_WeaponType.ranger){
            _weapon.Ammo= this.Ammo;
            _weapon.WeaponReLoadState   =this.WeaponReLoadState;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }
    }
    public void PipeToSkill(ref SkillData _skill){
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                _skill.SkillInfos[i].numericArray[j]   = this.SkillInfos[i].numericArray[j];
                _skill.SkillInfos[i].skillDelay[j]     = this.SkillInfos[i].skillDelay[j];
                _skill.SkillInfos[i].durateTime[j]     = this.SkillInfos[i].durateTime[j];
            }
            _skill.SkillUseState = this.SkillUseState[(int)_skill.CurrentSkillKey];
            _skill.SkillChangeState = this.SkillChangeState[(int)_skill.CurrentSkillKey];
            _skill.SkillLevelUpState = this.SkillLevelUpState[(int)_skill.CurrentSkillKey];
        }
    }
    //  데미지 공식을 적을 수 있다.
}