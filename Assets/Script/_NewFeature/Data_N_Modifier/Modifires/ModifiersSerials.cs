using UnityEngine;
namespace Sophia
{
    using Instantiates;
    using Sophia.Composite;
    using Sophia.Instantiates.Skills;

    #region Stat Modifier
    [System.Serializable]
    public struct SerialStatModifierDatas
    {
        public float amount;
        public E_STAT_CALC_TYPE calType;
    }

    [System.Serializable]
    public struct SerialStatCalculateDatas
    {
        [SerializeField] public SerialStatModifierDatas MaxHp;
        [SerializeField] public SerialStatModifierDatas Defence;
        [SerializeField] public SerialStatModifierDatas MoveSpeed;
        [SerializeField] public SerialStatModifierDatas Accecerate;
        [SerializeField] public SerialStatModifierDatas Tenacity;
        [SerializeField] public SerialStatModifierDatas MaxStamina;
        [SerializeField] public SerialStatModifierDatas StaminaRestoreSpeed;
        [SerializeField] public SerialStatModifierDatas Luck;
        [SerializeField] public SerialStatModifierDatas Power;
        [SerializeField] public SerialStatModifierDatas InstantiableDurateLifeTimeMultiplyRatio;
        [SerializeField] public SerialStatModifierDatas InstantiableSizeMultiplyRatio;
        [SerializeField] public SerialStatModifierDatas InstantiableForwardingSpeedMultiplyRatio;
        [SerializeField] public SerialStatModifierDatas PoolSize;
        [SerializeField] public SerialStatModifierDatas AttackSpeed;
        [SerializeField] public SerialStatModifierDatas MeleeRatio;
        [SerializeField] public SerialStatModifierDatas RangerRatio;
        [SerializeField] public SerialStatModifierDatas TechRatio;
        [SerializeField] public SerialStatModifierDatas EfficienceMultiplyer;
        [SerializeField] public SerialStatModifierDatas CoolDownSpeed;

        public SerialStatModifierDatas GetModifierDatas(E_NUMERIC_STAT_TYPE numericType)
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
                default: { return new SerialStatModifierDatas { amount = -1, calType = 0 }; }
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
    public struct SerialSkinData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public Material _materialRef;
    }
    
    [System.Serializable]
    public struct SerialVisualData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public VisualFXObject _visualFxRef;
    }

    [System.Serializable]
    public struct SerialTickDamageData
    {
        [SerializeField] public DamageInfo _baseTickDamage;
        [SerializeField] public float _intervalTime;
    }

    [System.Serializable]
    public struct SerialPhysicsData
    {
        [SerializeField] public float _intervalTime;
        [SerializeField] public float _physicsForce;
    }

    [System.Serializable]
    public struct SerialBarrierData {
        [SerializeField] public float     _barrierRatio;
    }

    [System.Serializable]
    public struct SerialAffectorData
    {
        [SerializeField] public SerialUserInterfaceData _uiData;
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _baseDurateTime;
        [SerializeField] public SerialSkinData                _skinData;
        [SerializeField] public SerialVisualData              _visualData;
        [SerializeField] public SerialBarrierData             _barrierData;
        [SerializeField] public SerialTickDamageData          _tickDamageData;
        [SerializeField] public SerialPhysicsData             _physicsData;
        [SerializeField] public SerialStatCalculateDatas      _calculateData;
        [SerializeField] public SerialProjectileVisualDatas   _projectileVisualData;
    }

    [System.Serializable]
    public struct SerialProjectileVisualDatas
    {
        [ColorUsage(true, true)]
        [SerializeField] public Color _shaderUnderbarColor;
        [SerializeField] public float _shaderUnderbarColorPower;
        [SerializeField] public VisualFXObject _destroyEffect;
        [SerializeField] public VisualFXObject _hitEffect;
    }

    public struct ProjectileVisualData {
        public Color ShaderUnderbarColor;
        public float ShaderUnderbarColorPower;
        public VisualFXObject DestroyEffect;
        public VisualFXObject HitEffect;
        
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
        [SerializeField] public float _damageRatio;
        [SerializeField] public DamageHandleType _damageHandleType;
        [SerializeField] public HitType _hitType;
    }
    
}