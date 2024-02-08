using UnityEngine;
namespace Sophia {
    using Instantiates;
    
    [System.Serializable]
    public struct SerialStatModifireDatas {
        public float amount;
        public E_STAT_CALC_TYPE calType;
    }

    [System.Serializable]
    public struct SerialEquipmentData {
        [SerializeField] public string _equipmentName;
        [SerializeField] public string _description;
        [SerializeField] public Sprite _icon;
        [SerializeField] public SerialCalculateDatas _calculateDatas;
    }
        [System.Serializable]
    public struct SerialTickDamageAffectData
    {
        [SerializeField] public float _baseTickDamage;
        [SerializeField] public float _tickDamageRatio;
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
        [SerializeField] public SerialCalculateDatas _calculateAffectData;
    }

}