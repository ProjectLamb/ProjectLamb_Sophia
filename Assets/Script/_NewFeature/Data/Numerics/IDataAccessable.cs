namespace Sophia
{
    using DataSystem;
    
    public enum E_STAT_USE_TYPE
    {
        None = 0,
        Natural = 1, Ratio, Percentage
    }

    public enum E_STAT_CALC_TYPE
    {
        None, Add, Mul
    }


    public enum E_NUMERIC_STAT_TYPE {
        None = 0,
        MaxHp = 1, Defence, Power, AttackSpeed, MoveSpeed, Tenacity, Accecerate,

        MaxStamina = 11, StaminaRestoreSpeed, Luck,

        InstantiableDurateLifeTimeMultiplyRatio = 21, InstantiableSizeMultiplyRatio, InstantiableForwardingSpeedMultiplyRatio,

        PoolSize = 31, MeleeRatio, RangerRatio, TechRatio,

        EfficienceMultiplyer = 41, CoolDownSpeed
    }

    public interface IStatAccessable {
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        public string GetStatsInfo();
    }
}