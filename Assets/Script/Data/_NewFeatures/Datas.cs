using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.Analytics;

namespace Feature_NewData
{
    public enum E_STAT_USE_TYPE {
        Fixed, Ratio
    }
    public enum E_STAT_CALC_TYPE  {
        MulBefore_1 = 1, Add_2, MulAfter_3
    }

    public class Stat {
        public readonly float BaseValue;
        public readonly E_STAT_USE_TYPE UseType;
        private float value;
        private bool isDirty;
        public float Value {
            get {
                if(isDirty) RecalculateValue();
                return value;
            }
        }

        private readonly List<StatCalculator> statCalculatorList = new();

        public Stat(float baseValue, E_STAT_USE_TYPE UseType){
            value = BaseValue = baseValue;
            this.UseType = UseType;
        }

        public static implicit operator int(Stat stat) {
            if(stat.UseType == E_STAT_USE_TYPE.Fixed)
                return (int) stat.Value;
            throw new System.Exception("Ratio 형식을 int로 리턴 불가.");
        }
        
        public static implicit operator float(Stat stat) {
            if(stat.UseType == E_STAT_USE_TYPE.Ratio)
                return stat.Value;
            throw new System.Exception("Value 형식을 float로 리턴 불가.");
        }

        public void AddCalculator(StatCalculator StatCalculator)
        {
            statCalculatorList.Add(StatCalculator);
            statCalculatorList.OrderBy(calc => calc.Order);
            isDirty = true;
        }

        public void RemoveCalculator(StatCalculator StatCalculator)
        {
            statCalculatorList.Remove(StatCalculator);
            isDirty = true;
        }
        
        public void ClearAllCalculators()
        {
            statCalculatorList.Clear();
            isDirty = true;
        }

        public void RecalculateValue()
        {
            value = BaseValue;
            statCalculatorList.ForEach( (calc) => {
                    if(calc.ClacType == E_STAT_CALC_TYPE.Add_2) {
                        this.value += calc.Value;
                    }
                    else {this.value *= value;}
                }
            );

            isDirty = false;            
        }
    }


    public class StatCalculator
    {   
        public readonly float Value;
        public readonly E_STAT_CALC_TYPE ClacType;
        public readonly int Order;
        
        public StatCalculator(float value, E_STAT_CALC_TYPE Type)
        {
            Value = value;
            ClacType = Type; 
            Order = (int) Type;
        }
    }

    /*********************************************************************************
    *
    * Data의 모든 수치가 
    * 양수로 되면 더 좋아진다라고 판단하면 됩니다.
    *
    *********************************************************************************/

    public struct EntityIdentifier
    {
        public int ID;
        public EntityType Type;
        public string Name;
    }
    
    public enum E_NUMERIC_STAT_MEMBERS {
        FixedMaxHp = 100, FixedDefence, FixedPower, FixedTenacity = 105, FixedMaxStamina, FixedLuck = 108,

        RatioMaxHp = 200 , RatioDefence , RatioPower , RatioAttackSpeed , RatioMoveSpeed , RatioTenacity, ReferingRatioStaminaRestoreSpeed = 207,
        RatioMaxDistance = 210, RatioMaxDuration, RatioSize, RatioSpeed,
        RatioMaxProjectilePoolSize = 220, RatioRestoreSpeed, RatioMeleeDamage, RatioRangeDamage, RatioTechDamage,
        RatioEfficienceMultiplyer = 230, RatioAccecerate
    }

    public enum E_FUNCTIONAL_ACTION_MEMBERS {
        Move = 0, Damaged, Attack, Affected, Dead, Standing, PhyiscTriggered, Skill, 
        
        Created = 10, Interacted,  Released, Forwarding, 
        WeaponUse = 20, WeaponChange, ProjectileRestore, 
        SkillUse = 30, SkillChange, SkillRefilled
    }
   
    public interface INumericAccessable
    {
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType);
        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, Stat stat);
    }

    public interface IFunctionalAccessable
    {
        public UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType);
        //public void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType);
    }

    /*
    float은 비율이다.
    int는 일반이다.
    */

    /*********************************************************************************
    *
    * Entity  Datas
    *
    *********************************************************************************/
    #region Entity  Datas

    public class EntityNumerics : INumericAccessable
    {
        public int MaxHp;           //OnDamaged
        public int Defence;       //OnDamaged
        public int Power;           //OnAttack
        public float AttackSpeed;     //OnAttack
        public float MoveSpeed;     //OnMove
        public int Tenacity;      //OnAffected
    }

    public class EntityFunctionals
    {
        public UnityAction OnMove;
        public UnityAction OnDamaged;
        public UnityAction OnAttack;
        public UnityAction OnAffected;
        public UnityAction OnDead;
        public UnityAction OnStanding;
        public UnityAction OnPhyiscTriggered;
    }

    public class EntityData : INumericAccessable<EntityNumerics>, IFunctionalAccessable<EntityFunctionals>
    {
        private EntityNumerics entityNumerics;
        private EntityFunctionals entityFunctionals;
        /*  Numeric     Data    */
        public EntityNumerics GetNumeric()
        {
            return null;
        }

        public void SetNumeric(EntityNumerics genericT)
        {

        }

        /*  Functional  Data    */
        public EntityFunctionals GetFunctional()
        {
            return null;
        }

        public void SetFunctional(EntityFunctionals genericT)
        {

        }
    }

    public class PlayerNumerics : EntityNumerics
    {
        public int MaxStamina;
        public float StaminaRestoreSpeed;
        public int Luck;
    }

    public class Wealths {
        public int Gear;
        public int Frag;
    }


    public class PlayerFunctionals
    {
        public UnityAction OnSkill;
    }

    public class PlayerData : INumericAccessable<PlayerNumerics>, IFunctionalAccessable<PlayerFunctionals>
    {
        private PlayerNumerics PlayerNumerics;
        private PlayerFunctionals PlayerFunctionals;
        /*  Numeric     Data    */
        public PlayerNumerics GetNumeric()
        {
            return null;
        }

        public void SetNumeric(PlayerNumerics genericT)
        {

        }

        /*  Functional  Data    */
        public PlayerFunctionals GetFunctional()
        {
            return null;
        }

        public void SetFunctional(PlayerFunctionals genericT)
        {

        }
    }

    #endregion

    /*********************************************************************************
    *
    * Carrier Instantiator  Datas
    *
    *********************************************************************************/
    #region Carrier Instantiator  Datas
    public class InstatiatorNumerics
    {
        public float MaxDistance;
        public float MaxDuration;
        public float Size;
        public float Speed;
    }

    public class InstatiatorFunctionals
    {
        public UnityAction OnCreated;
        public UnityAction OnInteracted;
        public UnityAction OnReleased;
        public UnityAction OnForwarding;
    }

    public class WeaponNumerics : InstatiatorNumerics
    {
        public int   MaxProjectilePoolSize;  // 근접 3단 공격이면 3개, 총알이면 30개
        public float RestoreSpeed;          // Projectile 회수딜레이
        public float MeleeDamageRatio;      // 근접공격 비례댐
        public float RangeDamageRatio;      // 원거리 공격 비례댐
        public float TechDamageRatio;       // 해킹/테크 공격 비례댐
    }

    public class WeaponFunctionals : InstatiatorFunctionals
    {
        public UnityAction OnWeaponUse;
        public UnityAction OnWeaponChange;
        public UnityAction OnProjectileRestore;
    }
    
    public class WeaponData : INumericAccessable<WeaponNumerics>, IFunctionalAccessable<WeaponFunctionals>
    {
        private WeaponNumerics WeaponNumerics;
        private WeaponFunctionals WeaponFunctionals;
        /*  Numeric     Data    */
        public WeaponNumerics GetNumeric()
        {
            return null;
        }

        public void SetNumeric(WeaponNumerics genericT)
        {

        }

        /*  Functional  Data    */
        public WeaponFunctionals GetFunctional()
        {
            return null;
        }

        public void SetFunctional(WeaponFunctionals genericT)
        {

        }
    }

    public class SkillNumerics : InstatiatorNumerics{
        public float EfficienceMultiplyer;
        public float Accecerate;
    }

    public class SkillFunctionals : InstatiatorFunctionals{
        public UnityAction OnUse;
        public UnityAction OnSkillChange;
        public UnityAction OnSkillRefilled;
    }

    public class SkillData : INumericAccessable<SkillNumerics>, IFunctionalAccessable<SkillFunctionals>
    {
        private SkillNumerics SkillNumerics;
        private SkillFunctionals SkillFunctionals;
        /*  Numeric     Data    */
        public SkillNumerics GetNumeric()
        {
            return null;
        }

        public void SetNumeric(SkillNumerics genericT)
        {

        }

        /*  Functional  Data    */
        public SkillFunctionals GetFunctional()
        {
            return null;
        }

        public void SetFunctional(SkillFunctionals genericT)
        {

        }
    }

    // 아이템 데이터는 매우 많기 때문에 Numerica Acceable과 FunctionalAcceable을 통해 가져와야한다... 
    // 적어도 PlayerData를 넘기는것은 맞아 보인다.
    // 이하 무기도 똑같다.
    // 적오도 PlayerData를 넘기는것은 맞아 보인다.
    #endregion

}