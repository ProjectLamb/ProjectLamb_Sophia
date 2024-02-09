using UnityEngine;
namespace Sophia {
    using Instantiates;
    using Sophia.Composite;

    [System.Serializable]
    public struct SerialStatModifireDatas {
        public float amount;
        public E_STAT_CALC_TYPE calType;
    }

    [System.Serializable]
    public struct SerialOnDamageExtrasModifierDatas {
        public E_FUNCTIONAL_EXTRAS_TYPE _extrasType;
        public E_EXTRAS_PERFORM_TYPE _performType;
        public SerialDamageConverterData _damageConverterData;
    }

    [System.Serializable]
    public struct SerialDamageConverterData {
        [SerializeField] public float _activatePercentage;
        [SerializeField] public E_AFFECT_TYPE _affectType;
        [SerializeField] public float _damageRatio;
        [SerializeField] public bool _criticalDamage;
        [SerializeField] public bool _dodgeDamage;
        [SerializeField] public bool _piercingDamage;
    }

    [System.Serializable]
    public struct SerialEquipmentData {
        [SerializeField] public string _equipmentName;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
        [SerializeField] public SerialStatCalculateDatas _statCalculateDatas;
        [SerializeField] public SerialExtrasCalculateDatas _extrasCalculateDatas;
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

}