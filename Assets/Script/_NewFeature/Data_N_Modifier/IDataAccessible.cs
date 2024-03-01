using System;

namespace Sophia
{
    using System.Collections.Generic;
    using DataSystem;
    using DataSystem.Referer;

    public enum E_STAT_USE_TYPE
    {
        None = 0,
        Natural = 1, Ratio, Percentage
    }

    public enum E_STAT_CALC_TYPE
    {
        None = 0, Add = 1 , Mul = 2
    }

    public enum E_NUMERIC_STAT_TYPE {
        None = 0,
        
        MaxHp = 1, Defence, Power, MoveSpeed, Accecerate, Tenacity, 

        MaxStamina = 11, StaminaRestoreSpeed, DashForce,

        InstantiableDurateLifeTimeMultiplyRatio = 21, InstantiableSizeMultiplyRatio, InstantiableForwardingSpeedMultiplyRatio,

        PoolSize = 31, AttackSpeed, MeleeRatio, RangerRatio, TechRatio,

        EfficienceMultiplyer = 41, CoolDownSpeed,

        Luck = 51
    }
    public enum E_EXTRAS_PERFORM_TYPE {
        None, Start, Tick, Exit
    }

    public enum E_FUNCTIONAL_EXTRAS_TYPE
    {
        None = 0,
        ENTITY_TYPE = 10,
            Move, Damaged, Attack, ConveyAffect, Dead, Idle, PhyiscTriggered,
        
        PLAYER_TYPE = 20,
            Dash, Skill,
        WEAPON_TYPE = 30, 
            WeaponUse, ProjectileRestore, WeaponConveyAffect, 
        
        SKILL_TYPE = 40,
            SkillUse, SkillRefilled, SkillConveyAffect,

        PROJECTILE_TYPE = 50,
            Created, Triggerd, Released, Forwarding,
    }

    public enum E_AFFECT_TYPE {
        None = 0,

        // 화상, 독, 출혈, 수축, 냉기, 혼란, 공포, 스턴, 속박, 처형
        // 블랙홀
        Debuff = 100,
        Burn, Poisoned, Bleed, Contracted, Cold, Confused, Fear, Stun, Bounded, 
        Knockback, BlackHole, Airborne, Execution,

        // 이동속도증가, 고유시간가속, 공격력증가, 보호막상태, CC저항, 은신, 무적, 방어/페링, 투사체생성, 회피,
        Buff = 200,
        MoveSpeedUp, Accelerated, PowerUp, Barrier, Resist, Invisible, Invincible, Defence, ProjectileGenerate, Dodgeing, 
    }   

    public interface IStatAccessible {
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        public string GetStatsInfo();
    }

    public interface IExtrasAccessible {
        public Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType);
    }

    public interface IDataAccessible : IStatAccessible, IExtrasAccessible {
        public EntityStatReferer GetStatReferer();
        public EntityExtrasReferer GetExtrasReferer();
    }

    public interface IDataSettable
    {
        public void SetStatDataToReferer(EntityStatReferer statReferer);
        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer);
    }
}