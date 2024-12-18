using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
using UnityEngine;

namespace Sophia.DB
{
    public interface ISkillDataAccessable
    {
            public int SkillID {get;}
            public E_SKILL_INDEX SkillType {get;}
            public string SkillName {get;}
            public string SkillDescription {get;}
            public Sprite SkillIcon {get;}
            public SerialUserInterfaceData SkillUserInterfaceData {get;} // 스킬 이름, 설명, 아이콘
            public SerialAffectorData SkillAffectorData {get;} // 자신에게 사용할 스텟 버프 디버프
            public SerialOnDamageExtrasModifierDatas SkillDamageModifierData {get;} // 자신에게 사용할 PowerUp 이외 공격력 증가 버프
            public SerialOnConveyAffectExtrasModifierDatas SkillConveyAffectModifierData {get; } // 적에게 주입할 스텟 버프 디버프
            public SerialProjectileInstantiateData SkillProjectileInstantiateData {get; } // 투사체 생성
            public SerialAudioData SkillActivatedAudioData {get; } // 스킬 발동시 사운드
    }

    [CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/Carrier/Item/Skill", order = int.MaxValue)]
    public class ScriptableSkillData : ScriptableObject ,ISkillDataAccessable
    {
        [SerializeField] private SerialSkillData _serialSkillData;

        public int SkillID => (int)_serialSkillData._index;
        public E_SKILL_INDEX SkillType => _serialSkillData._index;
        public string SkillName => _serialSkillData._userInterfaceData._name;
        public string SkillDescription => _serialSkillData._userInterfaceData._description;
        public Sprite SkillIcon => _serialSkillData._userInterfaceData._icon;

        public SerialUserInterfaceData SkillUserInterfaceData => _serialSkillData._userInterfaceData;
        public SerialAffectorData SkillAffectorData => _serialSkillData._affectorData;
        public SerialOnDamageExtrasModifierDatas SkillDamageModifierData => _serialSkillData._damageModifierData;
        public SerialOnConveyAffectExtrasModifierDatas SkillConveyAffectModifierData => _serialSkillData._conveyAffectModifierData;
        public SerialProjectileInstantiateData SkillProjectileInstantiateData => _serialSkillData._projectileInstantiateData;
        public SerialAudioData SkillActivatedAudioData => _serialSkillData._activatedAudioData;
    }
}