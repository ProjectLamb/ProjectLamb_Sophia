using System.Collections.Generic;
using System.Linq;

namespace Feature_NewData
{
    public enum E_NUMERIC_BASE_STATE_MENEBERS {
        
    }

    public enum E_NUMERIC_STAT_MEMBERS
    {
        FixedMaxHp = 100, FixedDefence, FixedPower, FixedTenacity = 105, FixedMaxStamina, FixedLuck = 108,

        RatioMaxHp = 200, RatioDefence, RatioPower, RatioAttackSpeed, RatioMoveSpeed, RatioTenacity, RatioStaminaRestoreSpeed = 207,
        RatioMaxDistance = 210, RatioMaxDuration, RatioSize, RatioSpeed,
        RatioMaxProjectilePoolSize = 220, RatioRestoreSpeed, RatioMeleeDamage, RatioRangeDamage, RatioTechDamage,
        RatioEfficienceMultiplyer = 230, RatioAccecerate
    }
    public interface INumericAccessable
    {
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType);
        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType);
        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);
        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);
        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType);
    }

    public abstract class Numerics : INumericAccessable
    {
        protected readonly Dictionary<E_NUMERIC_STAT_MEMBERS, Stat> numerics = new();

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return numerics[numericMemberType];
        }
        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType)
        {
            numerics[numericMemberType] = new Stat(value, useType);
        }
        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            numerics[numericMemberType].AddCalculator(calc);
        }
        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            numerics[numericMemberType].RemoveCalculator(calc);
        }
        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            numerics[numericMemberType].ResetCalculators();
        }
    }
}