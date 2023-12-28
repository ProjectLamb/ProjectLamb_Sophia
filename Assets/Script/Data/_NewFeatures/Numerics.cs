using System;

namespace Feature_NewData.Numerics
{
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
        private Stat DurateLifeTime         = null; // Ratio
        private Stat Size                   = null; // Ratio
        private Stat ForwardingSpeed        = null; // Ratio

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
                case E_NUMERIC_STAT_TYPE.DurateLifeTime :   { this.DurateLifeTime = statRef;    return; }
                case E_NUMERIC_STAT_TYPE.Size :             { this.Size = statRef;              return; }
                case E_NUMERIC_STAT_TYPE.ForwardingSpeed :  { this.ForwardingSpeed = statRef;   return; }
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
                case E_NUMERIC_STAT_TYPE.DurateLifeTime:    { res = this.DurateLifeTime;    break; }
                case E_NUMERIC_STAT_TYPE.Size:              { res = this.Size;              break; }
                case E_NUMERIC_STAT_TYPE.ForwardingSpeed:   { res = this.ForwardingSpeed;   break; }
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
                case E_NUMERIC_STAT_TYPE.DurateLifeTime:    { return this.DurateLifeTime == null ? -1 : this.DurateLifeTime.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.Size:              { return this.Size == null ? -1 : this.Size.GetValueForce(); }
                case E_NUMERIC_STAT_TYPE.ForwardingSpeed:   { return this.ForwardingSpeed == null ? -1 : this.ForwardingSpeed.GetValueForce(); }
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
        private Stat DurateLifeTime         = null;
        private Stat Size                   = null;
        private Stat ForwardingSpeed        = null;

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
                case E_NUMERIC_STAT_TYPE.DurateLifeTime         : {this.DurateLifeTime = statRef;return;}
                case E_NUMERIC_STAT_TYPE.Size                   : {this.Size = statRef;return;}
                case E_NUMERIC_STAT_TYPE.ForwardingSpeed        : {this.ForwardingSpeed = statRef;return;}
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
                case E_NUMERIC_STAT_TYPE.DurateLifeTime         : {res = this.DurateLifeTime; break;}
                case E_NUMERIC_STAT_TYPE.Size                   : {res = this.Size; break;}
                case E_NUMERIC_STAT_TYPE.ForwardingSpeed        : {res = this.ForwardingSpeed; break;}
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

    public struct Wealths
    {
        public int Gear;
        public int Frag;
    }
}