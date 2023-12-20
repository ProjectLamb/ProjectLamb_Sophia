using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

namespace Feature_NewData
{
    /*********************************************************************************
    *
    * Data의 모든 수치가 
    * 양수로 되면 더 좋아진다라고 판단하면 됩니다.
    *
    *********************************************************************************/
    public enum E_SYNERGY_MEMBERS 
    {
        Viking, Barbarian,Crusader,
        Cowboy
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

    public class EntityNumerics : Numerics
    {
        public EntityNumerics(){}
    }

    public class EntityData 
    {
        private EntityNumerics entityNumerics;
        /*  Numeric     Data    */
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return entityNumerics.GetNumeric(numericMemberType);
        }

        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            entityNumerics.AddCalculator(numericMemberType, calc);
        }

        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            entityNumerics.RemoveCalculator(numericMemberType, calc);
        }

        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            entityNumerics.ResetCalculators(numericMemberType);
        }

    }

    public class PlayerNumerics : Numerics
    {
        public PlayerNumerics(){}
    }

    public struct Wealths
    {
        public int Gear;
        public int Frag;
    }

    #endregion

    /*********************************************************************************
    *
    * Carrier Instantiator  Datas
    *
    *********************************************************************************/
    #region Carrier Instantiator  Datas
    public class InstatiatorNumerics : Numerics
    {
        public InstatiatorNumerics()
        {
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMaxDistance, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMaxDuration, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioSize, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));
        }
    }

    public class WeaponNumerics : InstatiatorNumerics
    {
        public WeaponNumerics() : base()
        {
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMaxProjectilePoolSize, new Stat(1f, E_STAT_USE_TYPE.Ratio));        // 근접 3단 공격이면 3개, 총알이면 30개
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioRestoreSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));                 // Projectile 회수딜레이
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMeleeDamage, new Stat(1f, E_STAT_USE_TYPE.Ratio));                  // 근접공격 비례댐
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioRangeDamage, new Stat(1f, E_STAT_USE_TYPE.Ratio));                  // 원거리 공격 비례댐
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioTechDamage, new Stat(1f, E_STAT_USE_TYPE.Ratio));                   // 해킹/테크 공격 비례댐
        }
    }

    public class WeaponData
    {
        private WeaponNumerics weaponNumerics;
        /*  Numeric     Data    */

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return weaponNumerics.GetNumeric(numericMemberType);
        }

        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            weaponNumerics.AddCalculator(numericMemberType, calc);
        }

        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            weaponNumerics.RemoveCalculator(numericMemberType, calc);
        }

        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            weaponNumerics.ResetCalculators(numericMemberType);
        }

    }

    public class SkillNumerics : InstatiatorNumerics
    {
        public SkillNumerics() : base()
        {
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioEfficienceMultiplyer, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioAccecerate, new Stat(1f, E_STAT_USE_TYPE.Ratio));
        }

    }


    public class SkillData 
    {
        public SkillNumerics skillNumerics;
        
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return skillNumerics.GetNumeric(numericMemberType);
        }

        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            skillNumerics.AddCalculator(numericMemberType, calc);
        }

        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            skillNumerics.RemoveCalculator(numericMemberType, calc);
        }

        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            skillNumerics.ResetCalculators(numericMemberType);
        }
    
    }

    
    // 적어도 PlayerData를 넘기는것은 맞아 보인다.
    // 이하 무기도 똑같다.
    // 적오도 PlayerData를 넘기는것은 맞아 보인다.
    #endregion
}