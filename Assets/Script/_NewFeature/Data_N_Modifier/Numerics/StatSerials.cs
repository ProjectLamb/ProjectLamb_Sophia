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
}