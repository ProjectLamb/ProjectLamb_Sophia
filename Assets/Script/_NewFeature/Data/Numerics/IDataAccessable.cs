namespace Sophia
{
    using DataSystem;
    using Sophia.DataSystem.Functional;
    using Sophia.DataSystem.Numerics;

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

    public interface IExtrasAccessable {
        public Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);
    }

    public interface IDataAccessable : IStatAccessable, IExtrasAccessable {
        public EntityStatReferer GetStatReferer();
        public EntityExtrasReferer GetExtrasReferer();
    }
}