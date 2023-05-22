using UnityEngine;
using UnityEngine.Events;

//모든 합연산같은 경우 다 파이프라인으로 진행한다.

/// <summary>
/// 특징 : 각 인스턴스마다 고유한 수치다. <br/>
/// 특징 : 데이터 전달을 하는데 사용한다. <br/>
/// Base + Pipeline 방식으로 사용 되므로 이 값이 최종 값이 되지는 않는다.   <br/>
/// 특징 :  어떤 데이터 타입과도 연산 가능하다 단, 결과물은 파이프라인이다.    <br/>
///       > Pipeline = EnityData      +*  PipelineData; <br/>
///       > Pipeline = PlayerData     +*  PipelineData; <br/>
///       > Pipeline = WeaponData     +*  PipelineData; <br/>
///       > Pipeline = SkillData      +*  PipelineData; <br/>
///       > Pipeline = PipelineData   +*  PipelineData; <br/>
/// 특징 : 파이프라인은 어떤 데이터와도 치환 가능하다.  <br/>
///     >  pipelineData.PipeToEntity(ref EntityData _data); <br/>
///     >  pipelineData.PipeToPlayer(ref PlayerData _data); <br/>
///     >  pipelineData.PipeToSkill(ref SkillData _data); <br/>
///     >  pipelineData.PipeToWeapon(ref WeaponData _data); <br/>
/// </summary>
/// 디버깅 : 모든 멤버를 ToString화 할 수 있다.
///     > pipeline.ToString();
[System.Serializable]
public class PipelineData {    
    [field : SerializeField] public int      MaxHP       {get; set;}
    [field : SerializeField] public int      CurHP       {get; set;}
    [field : SerializeField] public float    MoveSpeed   {get; set;}
    [field : SerializeField] public float    Defence     {get; set;}
    [field : SerializeField] public float    Tenacity    {get; set;}
    [field : SerializeField] public int      MaxStamina  {get; set;}
    [field : SerializeField] public int      CurStamina  {get; set;}
    [field : SerializeField] public float    StaminaRecoveryRation  {get; set;}
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
    public PipelineData(){
        MaxHP       = 0;
        CurHP       = 0;
        MoveSpeed   = 0f;
        Defence     = 0f;
        Tenacity    = 0f;
        MaxStamina  = 0;
        CurStamina  = 0;
        StaminaRecoveryRation = 0f;
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
    }
    public static PipelineData operator +(PipelineData x, EntityData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.Power   = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        return res;
    }
    public static PipelineData operator +(PipelineData x, PlayerData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.MaxStamina = x.MaxStamina +y.MaxStamina;
        res.CurStamina = x.CurStamina +y.CurStamina;
        res.Power   = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        res.Luck = x.Luck +y.Luck;
        res.Gear = x.Gear +y.Gear;
        res.Frag = x.Frag +y.Frag;
        return res;
    }
    public static PipelineData operator +(PipelineData x, WeaponData y)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio + y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay + y.WeaponDelay;
        res.Range           = x.Range + y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo + y.Ammo;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }
        return res;
    }
    public static PipelineData operator +(PipelineData x, SkillData y){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] + y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] + y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] + y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    public static PipelineData operator *(PipelineData x, EntityData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP *y.MaxHP;
        res.CurHP = x.CurHP *y.CurHP;
        res.MoveSpeed = x.MoveSpeed *y.MoveSpeed;
        res.Defence = x.Defence *y.Defence;
        res.Tenacity = x.Tenacity *y.Tenacity;
        res.Power = x.Power * y.Power;
        res.AttackSpeed = x.AttackSpeed * y.AttackSpeed;
        return res;
    }
    public static PipelineData operator *(PipelineData x, PlayerData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP *y.MaxHP;
        res.CurHP = x.CurHP *y.CurHP;
        res.MoveSpeed = x.MoveSpeed *y.MoveSpeed;
        res.Defence = x.Defence *y.Defence;
        res.Tenacity = x.Tenacity *y.Tenacity;
        res.MaxStamina = x.MaxStamina *y.MaxStamina;
        res.CurStamina = x.CurStamina *y.CurStamina;
        res.Power   = x.Power * y.Power;
        res.AttackSpeed = x.AttackSpeed * y.AttackSpeed;
        res.Luck = x.Luck *y.Luck;
        res.Gear = x.Gear *y.Gear;
        res.Frag = x.Frag *y.Frag;
        return res;
    }
    public static PipelineData operator *(PipelineData x, WeaponData y)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio * y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay * y.WeaponDelay;
        res.Range           = x.Range * y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo * y.Ammo;
        }
        return res;
    }
    public static PipelineData operator *(PipelineData x, SkillData y){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] * y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] * y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] * y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
        public static PipelineData operator +(EntityData x, PipelineData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.Power = x.Power + y.Power;
        res.AttackSpeed = x.AttackSpeed + y.AttackSpeed;
        return res;
    }
    public static PipelineData operator +(PlayerData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP +y.MaxHP;
        res.CurHP = x.CurHP +y.CurHP;
        res.MoveSpeed = x.MoveSpeed +y.MoveSpeed;
        res.Defence = x.Defence +y.Defence;
        res.Tenacity = x.Tenacity +y.Tenacity;
        res.MaxStamina = x.MaxStamina +y.MaxStamina;
        res.CurStamina = x.CurStamina +y.CurStamina;
        res.Power = x.Power +y.Power;
        res.AttackSpeed = x.AttackSpeed +y.AttackSpeed;
        res.Luck = x.Luck +y.Luck;
        res.Gear = x.Gear +y.Gear;
        res.Frag = x.Frag +y.Frag;
        return res;
    }
    public static PipelineData operator +(WeaponData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio + y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay + y.WeaponDelay;
        res.Range           = x.Range + y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo + y.Ammo;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }
        return res;
    }
    public static PipelineData operator +(SkillData y, PipelineData x){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] + y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] + y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] + y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    public static PipelineData operator *(EntityData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP *y.MaxHP;
        res.CurHP = x.CurHP *y.CurHP;
        res.MoveSpeed = x.MoveSpeed *y.MoveSpeed;
        res.Defence = x.Defence *y.Defence;
        res.Tenacity = x.Tenacity *y.Tenacity;        
        res.Power = x.Power * y.Power;
        res.AttackSpeed = x.AttackSpeed * y.AttackSpeed;
        return res;
    }
    public static PipelineData operator *(PlayerData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP *y.MaxHP;
        res.CurHP = x.CurHP *y.CurHP;
        res.MoveSpeed = x.MoveSpeed *y.MoveSpeed;
        res.Defence = x.Defence *y.Defence;
        res.Tenacity = x.Tenacity *y.Tenacity;
        res.MaxStamina = x.MaxStamina *y.MaxStamina;
        res.CurStamina = x.CurStamina *y.CurStamina;
        res.Power = x.Power *y.Power;
        res.AttackSpeed = x.AttackSpeed * y.AttackSpeed;
        res.Luck = x.Luck *y.Luck;
        res.Gear = x.Gear *y.Gear;
        res.Frag = x.Frag *y.Frag;
        return res;
    }
    public static PipelineData operator *(WeaponData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio * y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay * y.WeaponDelay;
        res.Range           = x.Range * y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo * y.Ammo;
        }
        return res;
    }
    public static PipelineData operator *(SkillData y, PipelineData x){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] * y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] * y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] * y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    
        public static PipelineData operator -(PipelineData x, EntityData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;
        return res;
    }
    public static PipelineData operator -(PipelineData x, PlayerData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;
        res.MaxStamina = x.MaxStamina -y.MaxStamina;
        res.CurStamina = x.CurStamina -y.CurStamina;
        res.Power = x.Power -y.Power;
        res.AttackSpeed = x.AttackSpeed -y.AttackSpeed;
        res.Luck = x.Luck -y.Luck;
        res.Gear = x.Gear -y.Gear;
        res.Frag = x.Frag -y.Frag;
        return res;
    }
    public static PipelineData operator -(PipelineData x, WeaponData y)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio - y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay - y.WeaponDelay;
        res.Range           = x.Range - y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo - y.Ammo;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }
        return res;
    }
    public static PipelineData operator -(PipelineData x, SkillData y){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] - y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] - y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] - y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    public static PipelineData operator /(PipelineData x, EntityData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP /((y.MaxHP < 0.001f)? 1 : y.MaxHP);
        res.CurHP = x.CurHP /((y.CurHP < 0.001f)? 1 : y.CurHP);
        res.MoveSpeed = x.MoveSpeed /((y.MoveSpeed < 0.001f)? 1 : y.MoveSpeed);
        res.Defence = x.Defence /((y.Defence < 0.001f)? 1 : y.Defence);
        res.Tenacity = x.Tenacity /((y.Tenacity < 0.001f)? 1 : y.Tenacity);
        res.Power = x.Power / ((y.Power < 0.001f)? 1 : y.Power);
        res.AttackSpeed = x.AttackSpeed / ((y.AttackSpeed < 0.001f)? 1 : y.AttackSpeed);
        return res;
    }
    public static PipelineData operator /(PipelineData x, PlayerData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP /((y.MaxHP < 0.001f) ? 1 : y.MaxHP);
        res.CurHP = x.CurHP /((y.CurHP < 0.001f) ? 1 : y.CurHP);
        res.MoveSpeed = x.MoveSpeed /((y.MoveSpeed < 0.001f) ? 1 : y.MoveSpeed);
        res.Defence = x.Defence /((y.Defence < 0.001f) ? 1 : y.Defence);
        res.Tenacity = x.Tenacity /((y.Tenacity < 0.001f) ? 1 : y.Tenacity);
        res.MaxStamina = x.MaxStamina /((y.MaxStamina < 0.001f) ? 1 : y.MaxStamina);
        res.CurStamina = x.CurStamina /((y.CurStamina < 0.001f) ? 1 : y.CurStamina);
        res.Power = x.Power / ((y.Power < 0.001f)? 1 : y.Power);
        res.AttackSpeed = x.AttackSpeed / ((y.AttackSpeed < 0.001f)? 1 : y.AttackSpeed);
        res.Luck = x.Luck /((y.Luck < 0.001f) ? 1 : y.Luck);
        res.Gear = x.Gear /((y.Gear < 0.001f) ? 1 : y.Gear);
        res.Frag = x.Frag /((y.Frag < 0.001f) ? 1 : y.Frag);
        return res;
    }
    public static PipelineData operator /(PipelineData x, WeaponData y)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio / ((y.DamageRatio < 0.001f) ? 1 : y.DamageRatio);
        res.WeaponDelay     = x.WeaponDelay / ((y.WeaponDelay < 0.001f) ? 1 : y.WeaponDelay);
        res.Range           = x.Range / ((y.Range < 0.001f) ? 1 : y.Range);
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo / ((y.Ammo < 0.001f) ? 1 : y.Ammo);
        }
        return res;
    }
    public static PipelineData operator /(PipelineData x, SkillData y){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] / ((y.SkillInfos[i].numericArray[j] < 0.001f) ? 1 : y.SkillInfos[i].numericArray[j]) ;
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] / ((y.SkillInfos[i].skillDelay[j] < 0.001f) ? 1 : y.SkillInfos[i].skillDelay[j]) ;
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] / ((y.SkillInfos[i].durateTime[j] < 0.001f) ? 1 : y.SkillInfos[i].durateTime[j]) ;
            }
        }
        return res;
    }
        public static PipelineData operator -(EntityData x, PipelineData y)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;
        return res;
    }
    public static PipelineData operator -(PlayerData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP -y.MaxHP;
        res.CurHP = x.CurHP -y.CurHP;
        res.MoveSpeed = x.MoveSpeed -y.MoveSpeed;
        res.Defence = x.Defence -y.Defence;
        res.Tenacity = x.Tenacity -y.Tenacity;
        res.MaxStamina = x.MaxStamina -y.MaxStamina;
        res.CurStamina = x.CurStamina -y.CurStamina;
        res.Power = x.Power - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;
        res.Luck = x.Luck -y.Luck;
        res.Gear = x.Gear -y.Gear;
        res.Frag = x.Frag -y.Frag;
        return res;
    }
    public static PipelineData operator -(WeaponData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio - y.DamageRatio;
        res.WeaponDelay     = x.WeaponDelay - y.WeaponDelay;
        res.Range           = x.Range - y.Range;
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo - y.Ammo;
        }
        else {
            Debug.Log("무기가 range가 아니면 Ammo는 더하지 않음");
        }
        return res;
    }
    public static PipelineData operator -(SkillData y, PipelineData x){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] - y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] - y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] - y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    public static PipelineData operator /(EntityData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP / ((y.MaxHP < 0.001f) ? 1 : y.MaxHP);
        res.CurHP = x.CurHP / ((y.CurHP < 0.001f) ? 1 : y.CurHP);
        res.MoveSpeed = x.MoveSpeed / ((y.MoveSpeed < 0.001f) ? 1 : y.MoveSpeed);
        res.Defence = x.Defence / ((y.Defence < 0.001f) ? 1 : y.Defence);
        res.Tenacity = x.Tenacity / ((y.Tenacity < 0.001f) ? 1 : y.Tenacity);
        res.Power = x.Power - ((y.Power < 0.001f) ? 1 : y.Power);
        res.AttackSpeed = x.AttackSpeed - ((y.AttackSpeed < 0.001f) ? 1 : y.AttackSpeed);
        return res;
    }
    public static PipelineData operator /(PlayerData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.MaxHP = x.MaxHP / ((y.MaxHP < 0.001f) ? 1 : y.MaxHP);
        res.CurHP = x.CurHP / ((y.CurHP < 0.001f) ? 1 : y.CurHP);
        res.MoveSpeed = x.MoveSpeed / ((y.MoveSpeed < 0.001f) ? 1 : y.MoveSpeed);
        res.Defence = x.Defence / ((y.Defence < 0.001f) ? 1 : y.Defence);
        res.Tenacity = x.Tenacity / ((y.Tenacity < 0.001f) ? 1 : y.Tenacity);
        res.MaxStamina = x.MaxStamina / ((y.MaxStamina < 0.001f) ? 1 : y.MaxStamina);
        res.CurStamina = x.CurStamina / ((y.CurStamina < 0.001f) ? 1 : y.CurStamina);
        res.Power = x.Power - ((y.Power < 0.001f) ? 1 : y.Power);
        res.AttackSpeed = x.AttackSpeed - ((y.AttackSpeed < 0.001f) ? 1 : y.AttackSpeed);
        res.Luck = x.Luck / ((y.Luck < 0.001f) ? 1 : y.Luck);
        res.Gear = x.Gear / ((y.Gear < 0.001f) ? 1 : y.Gear);
        res.Frag = x.Frag / ((y.Frag < 0.001f) ? 1 : y.Frag);
        return res;
    }
    public static PipelineData operator /(WeaponData y, PipelineData x)
    {
        PipelineData res = new PipelineData();
        res.DamageRatio     = x.DamageRatio / ((y.DamageRatio < 0.001f) ? 1 : y.DamageRatio);
        res.WeaponDelay     = x.WeaponDelay / ((y.WeaponDelay < 0.001f) ? 1 : y.WeaponDelay);
        res.Range           = x.Range / ((y.Range < 0.001f) ? 1 : y.Range);
        if(y.WeaponType == E_WeaponType.ranger){
            res.Ammo= x.Ammo / ((y.Ammo < 0.001f) ? 1 : y.Ammo);
        }
        return res;
    }
    public static PipelineData operator /(SkillData y, PipelineData x){
        PipelineData res = new PipelineData();
        for(int i = 0; i < 3; i++){
            for(int j =0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] / ((y.SkillInfos[i].numericArray[j] < 0.001f) ? 1 : y.SkillInfos[i].numericArray[j]);
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] / ((y.SkillInfos[i].skillDelay[j] < 0.001f) ? 1 : y.SkillInfos[i].skillDelay[j]);
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] / ((y.SkillInfos[i].durateTime[j] < 0.001f) ? 1 : y.SkillInfos[i].durateTime[j]);
            }
        }
        return res;
    }
    public static PipelineData operator +(PipelineData x, PipelineData y){
        PipelineData res = new PipelineData();
        res.MaxHP       = x.MaxHP       + y.MaxHP;
        res.CurHP       = x.CurHP       + y.CurHP;
        res.MoveSpeed   = x.MoveSpeed   + y.MoveSpeed;
        res.Defence     = x.Defence     + y.Defence;
        res.Tenacity    = x.Tenacity    + y.Tenacity;
        res.MaxStamina  = x.MaxStamina  + y.MaxStamina;
        res.CurStamina  = x.CurStamina  + y.CurStamina;
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
        return res;
    }
    public static PipelineData operator -(PipelineData x, PipelineData y){
        PipelineData res = new PipelineData();
        res.MaxHP       = x.MaxHP       - y.MaxHP;
        res.CurHP       = x.CurHP       - y.CurHP;
        res.MoveSpeed   = x.MoveSpeed   - y.MoveSpeed;
        res.Defence     = x.Defence     - y.Defence;
        res.Tenacity    = x.Tenacity    - y.Tenacity;
        res.MaxStamina  = x.MaxStamina  - y.MaxStamina;
        res.CurStamina  = x.CurStamina  - y.CurStamina;
        res.Power       = x.Power       - y.Power;
        res.AttackSpeed = x.AttackSpeed - y.AttackSpeed;
        res.Luck        = x.Luck        - y.Luck;
        res.Gear        = x.Gear        - y.Gear;
        res.Frag        = x.Frag        - y.Frag;
        res.DamageRatio = x.DamageRatio - y.DamageRatio;
        res.WeaponDelay = x.WeaponDelay - y.WeaponDelay;
        res.Range       = x.Range       - y.Range;
        res.Ammo        = x.Ammo        - y.Ammo;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] - y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] - y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] - y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }
    public static PipelineData operator *(PipelineData x, PipelineData y){
        PipelineData res = new PipelineData();
        res.MaxHP       = x.MaxHP       * y.MaxHP;
        res.CurHP       = x.CurHP       * y.CurHP;
        res.MoveSpeed   = x.MoveSpeed   * y.MoveSpeed;
        res.Defence     = x.Defence     * y.Defence;
        res.Tenacity    = x.Tenacity    * y.Tenacity;
        res.MaxStamina  = x.MaxStamina  * y.MaxStamina;
        res.CurStamina  = x.CurStamina  * y.CurStamina;
        res.Power       = x.Power       * y.Power;
        res.AttackSpeed = x.AttackSpeed * y.AttackSpeed;
        res.Luck        = x.Luck        * y.Luck;
        res.Gear        = x.Gear        * y.Gear;
        res.Frag        = x.Frag        * y.Frag;
        res.DamageRatio = x.DamageRatio * y.DamageRatio;
        res.WeaponDelay = x.WeaponDelay * y.WeaponDelay;
        res.Range       = x.Range       * y.Range;
        res.Ammo        = x.Ammo        * y.Ammo;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] * y.SkillInfos[i].numericArray[j];
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] * y.SkillInfos[i].skillDelay[j];
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] * y.SkillInfos[i].durateTime[j];
            }
        }
        return res;
    }

    public static PipelineData operator /(PipelineData x, PipelineData y){
        PipelineData res = new PipelineData();
        res.MaxHP       = x.MaxHP       / ((y.MaxHP < 0.001f) ? 1 : y.MaxHP);
        res.CurHP       = x.CurHP       / ((y.CurHP < 0.001f) ? 1 : y.CurHP);
        res.MoveSpeed   = x.MoveSpeed   / ((y.MoveSpeed < 0.001f) ? 1 : y.MoveSpeed);
        res.Defence     = x.Defence     / ((y.Defence < 0.001f) ? 1 : y.Defence);
        res.Tenacity    = x.Tenacity    / ((y.Tenacity < 0.001f) ? 1 : y.Tenacity);
        res.MaxStamina  = x.MaxStamina  / ((y.MaxStamina < 0.001f) ? 1 : y.MaxStamina);
        res.CurStamina  = x.CurStamina  / ((y.CurStamina < 0.001f) ? 1 : y.CurStamina);
        res.Power       = x.Power       / ((y.Power < 0.001f) ? 1 : y.Power);
        res.AttackSpeed = x.AttackSpeed / ((y.AttackSpeed < 0.001f) ? 1 : y.AttackSpeed);
        res.Luck        = x.Luck        / ((y.Luck < 0.001f) ? 1 : y.Luck);
        res.Gear        = x.Gear        / ((y.Gear < 0.001f) ? 1 : y.Gear);
        res.Frag        = x.Frag        / ((y.Frag < 0.001f) ? 1 : y.Frag);
        res.DamageRatio = x.DamageRatio / ((y.DamageRatio < 0.001f) ? 1 : y.DamageRatio);
        res.WeaponDelay = x.WeaponDelay / ((y.WeaponDelay < 0.001f) ? 1 : y.WeaponDelay);
        res.Range       = x.Range       / ((y.Range < 0.001f) ? 1 : y.Range);
        res.Ammo        = x.Ammo        / ((y.Ammo < 0.001f) ? 1 : y.Ammo);
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                res.SkillInfos[i].numericArray[j]   = x.SkillInfos[i].numericArray[j] / ((y.SkillInfos[i].numericArray[j] < 0.001f)? 1 : y.SkillInfos[i].numericArray[j]);
                res.SkillInfos[i].skillDelay[j]     = x.SkillInfos[i].skillDelay[j] / ((y.SkillInfos[i].skillDelay[j] < 0.001f)? 1 : y.SkillInfos[i].skillDelay[j]);
                res.SkillInfos[i].durateTime[j]     = x.SkillInfos[i].durateTime[j] / ((y.SkillInfos[i].durateTime[j] < 0.001f)? 1 : y.SkillInfos[i].durateTime[j]);
            }
        }
        return res;
    }

    public override string ToString(){
        string PlayerString = $"MaxHP : {MaxHP}, CurHP : {CurHP}, MoveSpeed : {MoveSpeed}, Defence : {Defence}, Tenacity : {Tenacity}, MaxStamina : {MaxStamina}, CurStamina : {CurStamina}, Power : {Power}, AttackSpeed : {AttackSpeed}, Luck : {Luck}, Gear : {Gear}, Frag : {Frag} \n";
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
    }
    public void PipeToPlayer(ref PlayerData _player){
        _player.MaxHP   = this.MaxHP;
        _player.CurHP   = this.CurHP;
        _player.MoveSpeed   = this.MoveSpeed;
        _player.Defence = this.Defence;
        _player.Tenacity    = this.Tenacity;
        _player.MaxStamina  = this.MaxStamina;
        _player.CurStamina  = this.CurStamina;
        _player.Power   = this.Power;
        _player.AttackSpeed = this.AttackSpeed;
        _player.Luck    = this.Luck;
        _player.Gear    = this.Gear;
        _player.Frag    = this.Frag;
    }
    public void PipeToWeapon(ref WeaponData _weapon){
        _weapon.DamageRatio     = this.DamageRatio;
        _weapon.WeaponDelay     = this.WeaponDelay;
        _weapon.Range           = this.Range;
        if(_weapon.WeaponType == E_WeaponType.ranger){
            _weapon.Ammo= this.Ammo;
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
        }
    }
}