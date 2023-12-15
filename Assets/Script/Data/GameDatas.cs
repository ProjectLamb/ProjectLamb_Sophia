using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Feature_NewData {
    public struct EntityIdentifier
    {
        public int ID;
        public EntityType Type;
        public string Name;
    }
    public interface INumericAccessable<T> {
        public T GetNumeric();
        public void SetNumeric(T genericT);
    }

    public interface IFunctionalAccessable<T> {
        public T GetNumeric();
        public void SetNumeric(T genericT);
    }

    public interface IDataAccessable<T> : INumericAccessable<T>, IFunctionalAccessable<T>{

    }

    /*
    float은 비율이다.
    int는 일반이다.
    */

    public class EntityData {
        /*  Numeric     Data    */
        public int MaxHp;           //OnDamaged
        public float Defence;       //OnDamaged
        public int Power;           //OnAttack
        public int AttackSpeed;     //OnAttack
        public float MoveSpeed;     //OnMove
        public float Tenacity;      //OnAffected

        /*  Functional  Data    */
        public UnityAction OnMove;
        public UnityAction OnDamaged;
        public UnityAction Attack;
        public UnityAction OnAffected;
        public UnityAction OnDead;
        public UnityAction OnUpdate;
        public UnityAction OnPhyiscTriggered;
    }

    public class PlayerData : EntityData {
        /*  Numeric     Data    */
        public int MaxStamina;
        public float StaminaRestoreRatio;
        public int Luck;
        public int Gear;
        public int Frag;

        /*  Functional  Data    */
        public UnityAction OnSkill;
    }

    public class PrefsData : EntityData {}
    public class TrapData : EntityData {}

    // 아이템 데이터는 매우 많기 때문에 Numerica Acceable과 FunctionalAcceable을 통해 가져와야한다... 
    // 이하 무기도 똑같다.
}