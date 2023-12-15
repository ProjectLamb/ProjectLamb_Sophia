using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

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
   
    public interface INumericAccessable<T>
    {
        public T GetNumeric();
        public void SetNumeric(T genericT);
    }

    public interface IFunctionalAccessable<T>
    {
        public T GetFunctional();
        public void SetFunctional(T genericT);
    }

    public interface IDataAccessable<T> : INumericAccessable<T>, IFunctionalAccessable<T>
    {

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

    public class EntityNumerics
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