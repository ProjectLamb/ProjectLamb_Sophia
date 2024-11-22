using System.Collections.Generic;
using Sophia.DB;
using Sophia.Instantiates;
using UnityEngine;

namespace Script.Tools
{
    public class AssignScriptableSkillData : MonoBehaviour
    {
        [SerializeField] private List<SkillItemObject> ConcreteSkillPrefabs;
        [SerializeField] private List<ScriptableSkillData> ScriptableSkillData;

        [ContextMenu("Assign")]
        public void TEST_ASSIGN()
        {
            int index = 0;
            foreach(var data in ConcreteSkillPrefabs) {
                data.Deep_Copy_To_Scriptable(ScriptableSkillData[index++]);
            }
        }
    }
}