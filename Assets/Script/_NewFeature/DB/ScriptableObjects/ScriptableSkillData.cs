using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
using UnityEngine;

namespace Sophia.DB
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/Carrier/Item/Skill", order = int.MaxValue)]
    public class ScriptableSkillData : ScriptableObject
    {
        [SerializeField] private SerialSkillData _serialSkillData;
        
        public void SetSerials(SerialSkillData serialSkillData)
        {
            _serialSkillData = serialSkillData;
        }
        
        public void SetSerial(
            E_SKILL_INDEX index,
            SerialUserInterfaceData userInterfaceData,  
            SerialAffectorData affectorData,  
            SerialOnDamageExtrasModifierDatas damageModifierData, 
            SerialOnConveyAffectExtrasModifierDatas conveyAffectModifierData, 
            SerialProjectileInstantiateData projectileInstantiateData, 
            SerialAudioData activatedAudioData
        )
        {
            SerialSkillData _tempSerialSkillData;
            _tempSerialSkillData._index = index;
            _tempSerialSkillData._userInterfaceData = userInterfaceData;
            _tempSerialSkillData._affectorData = affectorData;
            _tempSerialSkillData._damageModifierData = damageModifierData;
            _tempSerialSkillData._conveyAffectModifierData = conveyAffectModifierData;
            _tempSerialSkillData._projectileInstantiateData = projectileInstantiateData;
            _tempSerialSkillData._activatedAudioData = activatedAudioData;
            _serialSkillData = _tempSerialSkillData;
        }
    }
}