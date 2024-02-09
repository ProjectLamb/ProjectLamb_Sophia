using UnityEngine;

namespace Sophia
{
    [System.Serializable]
    public struct SerialBaseEntityData
    {

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
    public struct SerialBasePlayerData
    {
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
    public struct SerialBaseInstantiatorData
    {
        [Header("Instantiator/Comman")]
        public float InstantiableDurateLifeTimeMultiplyRatio;
        public float InstantiableSizeMultiplyRatio;
        public float InstantiableForwardingSpeedMultiplyRatio;
    }

    [System.Serializable]
    public struct SerialBaseWeaponData
    {
        [Header("Instantiator/Weapon")]
        public float PoolSize;
        public float AttackSpeed;
        public float MeleeRatio;
        public float RangerRatio;
        public float TechRatio;
    }

    [System.Serializable]
    public struct SerialBaseSkillData
    {
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

    [System.Serializable]
    public struct SerialStatCalculateDatas
    {
        [SerializeField] public SerialStatModifireDatas MaxHp;
        [SerializeField] public SerialStatModifireDatas Defence;
        [SerializeField] public SerialStatModifireDatas MoveSpeed;
        [SerializeField] public SerialStatModifireDatas Accecerate;
        [SerializeField] public SerialStatModifireDatas Tenacity;
        [SerializeField] public SerialStatModifireDatas MaxStamina;
        [SerializeField] public SerialStatModifireDatas StaminaRestoreSpeed;
        [SerializeField] public SerialStatModifireDatas Luck;
        [SerializeField] public SerialStatModifireDatas Power;
        [SerializeField] public SerialStatModifireDatas InstantiableDurateLifeTimeMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas InstantiableSizeMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas InstantiableForwardingSpeedMultiplyRatio;
        [SerializeField] public SerialStatModifireDatas PoolSize;
        [SerializeField] public SerialStatModifireDatas AttackSpeed;
        [SerializeField] public SerialStatModifireDatas MeleeRatio;
        [SerializeField] public SerialStatModifireDatas RangerRatio;
        [SerializeField] public SerialStatModifireDatas TechRatio;
        [SerializeField] public SerialStatModifireDatas EfficienceMultiplyer;
        [SerializeField] public SerialStatModifireDatas CoolDownSpeed;

        public SerialStatModifireDatas GetModifireDatas(E_NUMERIC_STAT_TYPE numericType)
        {
            switch (numericType)
            {
                case E_NUMERIC_STAT_TYPE.MaxHp: { return MaxHp; }
                case E_NUMERIC_STAT_TYPE.Defence: { return Defence; }
                case E_NUMERIC_STAT_TYPE.MoveSpeed: { return MoveSpeed; }
                case E_NUMERIC_STAT_TYPE.Accecerate: { return Accecerate; }
                case E_NUMERIC_STAT_TYPE.Tenacity: { return Tenacity; }
                case E_NUMERIC_STAT_TYPE.MaxStamina: { return MaxStamina; }
                case E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed: { return StaminaRestoreSpeed; }
                case E_NUMERIC_STAT_TYPE.Power: { return Power; }
                case E_NUMERIC_STAT_TYPE.Luck: { return Luck; }
                case E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio: { return InstantiableDurateLifeTimeMultiplyRatio; }
                case E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio: { return InstantiableSizeMultiplyRatio; }
                case E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio: { return InstantiableForwardingSpeedMultiplyRatio; }
                case E_NUMERIC_STAT_TYPE.PoolSize: { return PoolSize; }
                case E_NUMERIC_STAT_TYPE.AttackSpeed: { return AttackSpeed; }
                case E_NUMERIC_STAT_TYPE.MeleeRatio: { return MeleeRatio; }
                case E_NUMERIC_STAT_TYPE.RangerRatio: { return RangerRatio; }
                case E_NUMERIC_STAT_TYPE.TechRatio: { return TechRatio; }
                case E_NUMERIC_STAT_TYPE.EfficienceMultiplyer: { return EfficienceMultiplyer; }
                case E_NUMERIC_STAT_TYPE.CoolDownSpeed: { return CoolDownSpeed; }
                default: { return new SerialStatModifireDatas { amount = -1, calType = 0 }; }
            }
        }
    }

}