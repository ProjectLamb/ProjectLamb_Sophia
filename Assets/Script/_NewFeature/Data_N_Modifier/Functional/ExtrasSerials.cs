using UnityEngine;
namespace Sophia {
    using Instantiates;
    using Sophia.Composite;

    [System.Serializable]
    public struct SerialExtrasCalculateDatas {
        [SerializeField] public SerialOnDamageExtrasModifierDatas OnDamaged;
        [SerializeField] public SerialOnConveyAffectExtrasModifierDatas OnConveyAffect;
    }
}