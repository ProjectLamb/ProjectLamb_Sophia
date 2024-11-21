using UnityEngine;

namespace Sophia.DB
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObject/Carrier/Item/Skill", order = int.MaxValue)]
    public class ScriptableSkillData : ScriptableObject
    {
        [SerializeField] public SerialSkillData _serialSkillData;
    }
}