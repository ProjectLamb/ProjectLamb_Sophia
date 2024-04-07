using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Instantiates
{
    using Skills;

    [System.Serializable]
    public struct SkillCardData
    {
        public bool IsAvailable;
        [SerializeField] public E_SKILL_INDEX[] _index;
        [SerializeField] public SerialUserInterfaceData     _userInterfaceData;
        [SerializeField] public SerialAffectorData          _affectorData;
        [SerializeField] public SerialOnDamageExtrasModifierDatas _damageModifierData;
        [SerializeField] public SerialOnConveyAffectExtrasModifierDatas _conveyAffectModifierData;
        [SerializeField] public SerialProjectileInstantiateData _projectileInstantiateData;
    }
}
