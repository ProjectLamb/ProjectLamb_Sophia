using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Sophia.DB
{
    public class GlobalScriptableSkillModelManager : MonoBehaviour
    {
        [SerializedDictionary("SkillIndex", "ScriptableSkillData")] [field: SerializeField]
        private SerializedDictionary<int, ScriptableSkillData> _scriptableSkillDatas;
        public IReadOnlyDictionary<int, ScriptableSkillData> ScriptableSkillDatas => _scriptableSkillDatas;
    }
}
