namespace Sophia
{
    using DataSystem;
    
    public enum E_STAT_USE_TYPE
    {
        None = 0,
        Natural = 1, Ratio
    }
    public enum E_STAT_CALC_TYPE
    {
        Add, Mul
    }


    public enum E_NUMERIC_STAT_TYPE {
        None = 0,
        MaxHp = 1, Defence, Power, AttackSpeed, MoveSpeed, Tenacity, Accecerate,

        MaxStamina = 11, StaminaRestoreSpeed, Luck,

        DurateLifeTime = 21, Size, ForwardingSpeed,

        PoolSize = 31, MeleeRatio, RangerRatio, TechRatio,

        EfficienceMultiplyer = 41, CoolDownSpeed
    }
    public enum E_FUNCTIONAL_ACTION_MEMBERS
    {
        Move = 0, Damaged, Attack, Affected, Dead, Standing, PhyiscTriggered, Skill,

        Created = 10, Triggerd, Released, Forwarding,
        WeaponUse = 20, ProjectileRestore,
        SkillUse = 30, SkillRefilled
    }

    public interface IStatAccessable {
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        public string GetStatsInfo();
    }
}