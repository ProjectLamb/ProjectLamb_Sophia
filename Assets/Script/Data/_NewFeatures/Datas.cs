using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using Feature_NewData;

namespace Feature_NewData
{
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




    public enum E_SYNERGY_MEMBERS 
    {
        Viking, Barbarian,Crusader,
        Cowboy
    }

    public interface IDataAccessable : INumericAccessable, IFunctionalAccessable { }

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
        public EntityNumerics()
        {
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedMaxHp, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedDefence, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedPower, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedTenacity, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedMaxStamina, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedLuck, new Stat(0, E_STAT_USE_TYPE.Fixed));

            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMaxHp, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioDefence, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioPower, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioAttackSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioMoveSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioTenacity, new Stat(1f, E_STAT_USE_TYPE.Ratio));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioStaminaRestoreSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));
        }
    }

    public class EntityFunctionals
    {
        ExtraAction<bool>        moveExtra;
        ExtraAction<int>         damagedExtra;
        ExtraAction<bool>        attackExtra;
        ExtraAction<bool>        deadExtra;
        ExtraAction<bool>        standingExtra;
        ExtraAction<Collision>   phyiscTriggeredExtra;

        public EntityFunctionals()
        {
            ExtraAction<bool>        moveExtra               = new ExtraAction<bool>();
            ExtraAction<int>         damagedExtra            = new ExtraAction<int>();
            ExtraAction<bool>        attackExtra             = new ExtraAction<bool>();
            ExtraAction<bool>        deadExtra               = new ExtraAction<bool>();
            ExtraAction<bool>        standingExtra           = new ExtraAction<bool>();
            ExtraAction<Collision>   phyiscTriggeredExtra    = new ExtraAction<Collision>();
        }

        public void TempFunction(){
            Dictionary<E_EXTRA_PERFORM_TYPE, UnityActionRef<bool>> voidExtras = moveExtra;
            bool voidBool = true;
            voidExtras[E_EXTRA_PERFORM_TYPE.Start].Invoke(ref voidBool);
        }
    }

    public class EntityData : IDataAccessable
    {
        private EntityNumerics entityNumerics;
        private EntityFunctionals entityFunctionals;
        /*  Numeric     Data    */
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return entityNumerics.GetNumeric(numericMemberType);
        }

        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType)
        {
            entityNumerics.SetNumeric(numericMemberType, value, useType);
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

        public UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType)
        {
            return entityFunctionals.GetFunctional(functionalActionType);
        }

        public void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType, UnityAction action)
        {
            entityFunctionals.SetFunctional(functionalActionType, action);
        }
    }

    public class PlayerNumerics : Numerics
    {
        public PlayerNumerics()
        {
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedMaxStamina, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.FixedLuck, new Stat(0, E_STAT_USE_TYPE.Fixed));
            numerics.Add(E_NUMERIC_STAT_MEMBERS.RatioStaminaRestoreSpeed, new Stat(1f, E_STAT_USE_TYPE.Ratio));
        }
    }

    public struct Wealths
    {
        public int Gear;
        public int Frag;
    }


    public class PlayerFunctionals : Functional
    {
        public PlayerFunctionals()
        {
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.Skill, () => { });
        }
    }

    public class PlayerData : IDataAccessable
    {
        private PlayerNumerics playerNumerics;
        private PlayerFunctionals playerFunctionals;

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return playerNumerics.GetNumeric(numericMemberType);
        }

        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType)
        {
            playerNumerics.SetNumeric(numericMemberType, value, useType);
        }

        public void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            playerNumerics.AddCalculator(numericMemberType, calc);
        }

        public void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc)
        {
            playerNumerics.RemoveCalculator(numericMemberType, calc);
        }

        public void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            playerNumerics.ResetCalculators(numericMemberType);
        }

        public UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType)
        {
            return playerFunctionals.GetFunctional(functionalActionType);
        }

        public void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType, UnityAction action)
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

    public class InstatiatorFunctionals : Functional
    {
        public InstatiatorFunctionals()
        {
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.Created, () => { });
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.Triggerd, () => { });
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.Released, () => { });
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.Forwarding, () => { });
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

    public class WeaponFunctionals : InstatiatorFunctionals
    {
        public WeaponFunctionals()
        {
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.WeaponUse, () => { });
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.ProjectileRestore, () => { });
        }
    }

    public class WeaponData : IDataAccessable
    {
        private WeaponNumerics weaponNumerics;
        private WeaponFunctionals weaponFunctionals;
        /*  Numeric     Data    */

        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return weaponNumerics.GetNumeric(numericMemberType);
        }

        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType)
        {
            weaponNumerics.SetNumeric(numericMemberType, value, useType);
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

        public UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType)
        {
            return weaponFunctionals.GetFunctional(functionalActionType);
        }

        public void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType, UnityAction action)
        {
            weaponFunctionals.SetFunctional(functionalActionType, action);
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

    public class SkillFunctionals : InstatiatorFunctionals
    {
        public SkillFunctionals()
        {
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.SkillUse, () => { });
            functionals.Add(E_FUNCTIONAL_ACTION_MEMBERS.SkillRefilled, () => { });
        }
    }

    public class SkillData : IDataAccessable
    {
        public SkillNumerics skillNumerics;
        public SkillFunctionals skillFunctionals;
        public Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType)
        {
            return skillNumerics.GetNumeric(numericMemberType);
        }
        public void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType)
        {
            skillNumerics.SetNumeric(numericMemberType, value, useType);
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
        public UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType)
        {
            return skillFunctionals.GetFunctional(functionalActionType);
        }

        public void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType, UnityAction action)
        {
            skillFunctionals.SetFunctional(functionalActionType, action);
        }


    }

    // 아이템 데이터는 매우 많기 때문에 Numerica Acceable과 FunctionalAcceable을 통해 가져와야한다... 
    // 적어도 PlayerData를 넘기는것은 맞아 보인다.
    // 이하 무기도 똑같다.
    // 적오도 PlayerData를 넘기는것은 맞아 보인다.
    #endregion

}