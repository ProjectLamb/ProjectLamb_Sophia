using UnityEngine;
namespace Sophia
{
    using Instantiates;
    using Sophia.Composite;

#region Stat Modifier
    [System.Serializable]
    public struct SerialStatModifireDatas
    {
        public float amount;
        public E_STAT_CALC_TYPE calType;
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

#endregion

#region Extras Modifier
#endregion

#region Equipment

    [System.Serializable]
    public struct SerialEquipmentData
    {
        [SerializeField] public int _equipmentID;
        [SerializeField] public string _equipmentName;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
        [SerializeField] public SerialStatCalculateDatas _statCalculateDatas;
        [SerializeField] public SerialExtrasCalculateDatas _extrasCalculateDatas;
    }
    
#endregion

#region Affector

    [System.Serializable]
    public struct SerialSkinAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public Material _materialRef;
    }
    [System.Serializable]
    public struct SerialVisualAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public VisualFXObject _visualFxRef;
    }

    [System.Serializable]
    public struct SerialTickDamageAffectData
    {
        [SerializeField] public DamageInfo _baseTickDamage;
        [SerializeField] public float _intervalTime;
    }

    [System.Serializable]
    public struct SerialPhysicsAffectData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public float _physicsForce;
    }

    [System.Serializable]
    public struct SerialAffectorData
    {
        [SerializeField] public string _equipmentName;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _baseDurateTime;
        [SerializeField] public SerialSkinAffectData _skinAffectData;
        [SerializeField] public SerialVisualAffectData _visualAffectData;
        [SerializeField] public SerialTickDamageAffectData _tickDamageAffectData;
        [SerializeField] public SerialPhysicsAffectData _physicsAffectData;
        [SerializeField] public SerialStatCalculateDatas _calculateAffectData;
    }


#endregion

    [System.Serializable]
    public struct SerialOnDamageExtrasModifierDatas
    {
        public E_FUNCTIONAL_EXTRAS_TYPE _extrasType;
        public E_EXTRAS_PERFORM_TYPE _performType;
        public SerialDamageConverterData _damageConverterData;
    }
    [System.Serializable]
    public struct SerialOnConveyAffectExtrasModifierDatas 
    {
        public E_FUNCTIONAL_EXTRAS_TYPE _extrasType;
        public E_EXTRAS_PERFORM_TYPE _performType;
        public SerialAffectorData _affectData;
    }

    [System.Serializable]
    public struct SerialDamageConverterData
    {
        [SerializeField] public float _activatePercentage;
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _damageRatio;
        [SerializeField] public DamageHandleType _damageHandleType;
        [SerializeField] public HitType _hitType;
    }
}