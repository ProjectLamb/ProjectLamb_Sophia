using System;
using UnityEngine;

namespace Sophia.DataSystem.Numerics
{
    [System.Serializable]
    public struct SerialBaseEntityData{
    
    [Header("Life ")]
        public float MaxHp;
        public float Defence;
    
    [Header("Move")]
        public float MoveSpeed;
        public float Accecerate;
    
    [Header("Affect")]
        public float Tenacity;
    
    [Header("Instantiator")]
        public float Power;

        public float InstantiableDurateLifeTimeMultiplyRatio;
        public float InstantiableSizeMultiplyRatio;
        public float InstantiableForwardingSpeedMultiplyRatio;
    }
    
    [System.Serializable]
    public struct SerialBasePlayerData{
        [Header("Life ")]
        public float MaxHp;
        public float Defence;
        [Header("Move")]
        public float MoveSpeed;
        public float Accecerate;
        [Header("Affect")]
        public float Tenacity;

        [Header("Dash")]
        public float MaxStamina;
        public float StaminaRestoreSpeed;

        [Header("Ohter")]
        public float Luck;

        [Header("Instantiator")]
        public float Power;
    }
    
    [System.Serializable]
    public struct SerialBaseInstantiatorData {
        [Header("Instantiator/Comman")]
        public float InstantiableDurateLifeTimeMultiplyRatio;
        public float InstantiableSizeMultiplyRatio;
        public float InstantiableForwardingSpeedMultiplyRatio;
    }
    
    [System.Serializable]
    public struct SerialBaseWeaponData {
        [Header("Instantiator/Weapon")]
        public float PoolSize;
        public float AttackSpeed;
        public float MeleeRatio;
        public float RangerRatio;
        public float TechRatio;
    }
    
    [System.Serializable]
    public struct SerialBaseSkillData {
        [Header("Instantiator/Skill")]
        public float EfficienceMultiplyer;
        public float CoolDownSpeed;
    }

    [System.Serializable]
    public struct Wealths
    {
        public int Gear;
        public int Frag;
    }

    public class EntityStatReferer : IStatAccessable
    {

#region Life 
        private Stat MaxHp                  = null; // Natural
        private Stat Defence                = null; //Natural to Function

#endregion

#region Move

        private Stat MoveSpeed              = null; //Natural
        private Stat Accecerate             = null; // Ratio 

#endregion

#region Affect

        private Stat Tenacity               = null; // Natural to Fcuntion

#endregion

#region Instantiator

        private Stat Power                  = null; // Natural
        private Stat InstantiableDurateLifeTimeMultiplyRatio         = null; // Ratio
        private Stat InstantiableSizeMultiplyRatio                   = null; // Ratio
        private Stat InstantiableForwardingSpeedMultiplyRatio        = null; // Ratio

#endregion

        public virtual void SetRefStat(Stat statRef)
        {
            switch(statRef.NumericType) 
            {
                case E_NUMERIC_STAT_TYPE.MaxHp :            { this.MaxHp = statRef;             return; }
                case E_NUMERIC_STAT_TYPE.Defence :          { this.Defence = statRef;           return; }
                case E_NUMERIC_STAT_TYPE.MoveSpeed :        { this.MoveSpeed = statRef;         return; }
                case E_NUMERIC_STAT_TYPE.Accecerate :       { this.Accecerate = statRef;        return; }
                case E_NUMERIC_STAT_TYPE.Tenacity :         { this.Tenacity = statRef;          return; }
                case E_NUMERIC_STAT_TYPE.Power :            { this.Power = statRef;             return; }
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio :   { this.InstantiableDurateLifeTimeMultiplyRatio = statRef;    return; }
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio :             { this.InstantiableSizeMultiplyRatio = statRef;              return; }
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio :  { this.InstantiableForwardingSpeedMultiplyRatio = statRef;   return; }
                case E_NUMERIC_STAT_TYPE.None : {
                    throw new System.Exception($"참조 스텟이 초기화되지 않음");
                }
                default : {
                    throw new System.Exception($"이 Entity 멤버에는 {statRef.NumericType.ToString()} 없음");
                }
            }
        }

        public virtual Stat GetStat(E_NUMERIC_STAT_TYPE numericType)
        {
            Stat res = null;
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp:             { res = this.MaxHp;             break; }
                case E_NUMERIC_STAT_TYPE.Defence:           { res = this.Defence;           break; }
                case E_NUMERIC_STAT_TYPE.MoveSpeed:         { res = this.MoveSpeed;         break; }
                case E_NUMERIC_STAT_TYPE.Accecerate:        { res = this.Accecerate;        break; }
                case E_NUMERIC_STAT_TYPE.Tenacity:          { res = this.Tenacity;          break; }
                case E_NUMERIC_STAT_TYPE.Power:             { res = this.Power;             break; }
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio:    { res = this.InstantiableDurateLifeTimeMultiplyRatio;    break; }
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio:              { res = this.InstantiableSizeMultiplyRatio;              break; }
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio:   { res = this.InstantiableForwardingSpeedMultiplyRatio;   break; }
                default: {
                    throw new System.Exception($"이 Entity 멤버에는 {numericType.ToString()} 없음");
                }
            }
            if(res == null) {
                throw new System.Exception($"참조하려는 {numericType.ToString()} 멤버가 초기화되지 않음");
            }
            return res;
        }

        protected virtual float GetStatByValues(E_NUMERIC_STAT_TYPE numericType)
        {
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp:             { return this.MaxHp == null ? -1 : this.MaxHp.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.Defence:           { return this.Defence == null ? -1 : this.Defence.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.MoveSpeed:         { return this.MoveSpeed == null ? -1 : this.MoveSpeed.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.Accecerate:        { return this.Accecerate == null ? -1 : this.Accecerate.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.Tenacity:          { return this.Tenacity == null ? -1 : this.Tenacity.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.Power:             { return this.Power == null ? -1 : this.Power.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio:    { return this.InstantiableDurateLifeTimeMultiplyRatio == null ? -1 : this.InstantiableDurateLifeTimeMultiplyRatio.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio:              { return this.InstantiableSizeMultiplyRatio == null ? -1 : this.InstantiableSizeMultiplyRatio.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio:   { return this.InstantiableForwardingSpeedMultiplyRatio == null ? -1 : this.InstantiableForwardingSpeedMultiplyRatio.GetValueForce(); }
                default: {
                    return -1;
                }
            }
        }

        public virtual string GetStatsInfo()
        {
            string res = "";
            foreach(E_NUMERIC_STAT_TYPE Enum in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
            {
                float val = this.GetStatByValues(Enum);
                if(val < 0 ) {continue;}
                res += $"{Enum.ToString()} : {val} \n";
            }
            return res;
        }
    }

    public class PlayerStatReferer : EntityStatReferer
    {
#region Life 
        private Stat MaxHp                  = null; // Natural
        private Stat Defence                = null; //Natural to Function

#endregion

#region Move

        private Stat MoveSpeed              = null; //Natural
        private Stat Accecerate             = null; // Ratio 

#endregion

#region Affect

        private Stat Tenacity               = null; // Natural to Fcuntion

#endregion

#region Dash
        private Stat MaxStamina             = null;
        private Stat StaminaRestoreSpeed    = null;
#endregion

#region CoolTime
        private Stat CoolDownSpeed          = null;
#endregion

#region Instantiator


/******************************
* Common
******************************/

        private Stat Power                  = null; // Natural
        private Stat InstantiableDurateLifeTimeMultiplyRatio         = null;
        private Stat InstantiableSizeMultiplyRatio                   = null;
        private Stat InstantiableForwardingSpeedMultiplyRatio        = null;

/******************************
* Weapon Only
******************************/
        private Stat PoolSize               = null;
        private Stat AttackSpeed            = null; //Ratio, 재생 속도
        private Stat MeleeRatio             = null;
        private Stat RangerRatio            = null;
        private Stat TechRatio              = null;

/******************************
* Skill Only
******************************/

        private Stat EfficienceMultiplyer   = null;

#endregion

#region Other

        private Stat Luck                   = null;

        #endregion
        public override void SetRefStat(Stat statRef)
        {
            switch(statRef.NumericType) 
            {
                case E_NUMERIC_STAT_TYPE.MaxHp                  : {this.MaxHp = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Defence                : {this.Defence = statRef;return;}
                case E_NUMERIC_STAT_TYPE.MoveSpeed              : {this.MoveSpeed = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Accecerate             : {this.Accecerate = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Tenacity               : {this.Tenacity = statRef;return;}
                case E_NUMERIC_STAT_TYPE.MaxStamina             : {this.MaxStamina = statRef;return;}
                case E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed    : {this.StaminaRestoreSpeed = statRef;return;}
                case E_NUMERIC_STAT_TYPE.CoolDownSpeed          : {this.CoolDownSpeed = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Power                  : {this.Power = statRef;return;}
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio         : {this.InstantiableDurateLifeTimeMultiplyRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio                   : {this.InstantiableSizeMultiplyRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio        : {this.InstantiableForwardingSpeedMultiplyRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.PoolSize               : {this.PoolSize = statRef;return;}
                case E_NUMERIC_STAT_TYPE.AttackSpeed            : {this.AttackSpeed = statRef;return;}
                case E_NUMERIC_STAT_TYPE.MeleeRatio             : {this.MeleeRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.RangerRatio            : {this.RangerRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.TechRatio              : {this.TechRatio = statRef;return;}
                case E_NUMERIC_STAT_TYPE.EfficienceMultiplyer   : {this.EfficienceMultiplyer = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Luck                   : {this.Luck = statRef;return;}
                case E_NUMERIC_STAT_TYPE.None : {
                    throw new System.Exception($"참조 스텟이 초기화되지 않음");
                }
                default : {
                    throw new System.Exception($"이 Entity 멤버에는 {statRef.NumericType.ToString()} 없음");
                }
            }
        }

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType)
        {
            Stat res = null;
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp                  : {res = this.MaxHp; break;}
                case E_NUMERIC_STAT_TYPE.Defence                : {res = this.Defence; break;}
                case E_NUMERIC_STAT_TYPE.MoveSpeed              : {res = this.MoveSpeed; break;}
                case E_NUMERIC_STAT_TYPE.Accecerate             : {res = this.Accecerate; break;}
                case E_NUMERIC_STAT_TYPE.Tenacity               : {res = this.Tenacity; break;}
                case E_NUMERIC_STAT_TYPE.MaxStamina             : {res = this.MaxStamina; break;}
                case E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed    : {res = this.StaminaRestoreSpeed; break;}
                case E_NUMERIC_STAT_TYPE.CoolDownSpeed          : {res = this.CoolDownSpeed; break;}
                case E_NUMERIC_STAT_TYPE.Power                  : {res = this.Power; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio         : {res = this.InstantiableDurateLifeTimeMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio                   : {res = this.InstantiableSizeMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio        : {res = this.InstantiableForwardingSpeedMultiplyRatio; break;}
                case E_NUMERIC_STAT_TYPE.PoolSize               : {res = this.PoolSize; break;}
                case E_NUMERIC_STAT_TYPE.AttackSpeed            : {res = this.AttackSpeed; break;}
                case E_NUMERIC_STAT_TYPE.MeleeRatio             : {res = this.MeleeRatio; break;}
                case E_NUMERIC_STAT_TYPE.RangerRatio            : {res = this.RangerRatio; break;}
                case E_NUMERIC_STAT_TYPE.TechRatio              : {res = this.TechRatio; break;}
                case E_NUMERIC_STAT_TYPE.EfficienceMultiplyer   : {res = this.EfficienceMultiplyer; break;}
                case E_NUMERIC_STAT_TYPE.Luck                   : {res = this.Luck; break;}
                default: {
                    throw new System.Exception($"이 Entity 멤버에는 {numericType.ToString()} 없음");
                }
            }

            if(res == null) {
                throw new System.Exception($"참조하려는 {numericType.ToString()} 멤버가 초기화되지 않음");
            }
            return res;
        }
    }
}