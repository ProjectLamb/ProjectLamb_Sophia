using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sophia;
using Sophia.DB;
using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
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
        [ContextMenu("Overlap")]
        public void TEST_OVERLAP()
        {
            int index = 0;
            List<E_SKILL_INDEX> skillEnum = new List<E_SKILL_INDEX>();
            foreach(E_SKILL_INDEX e in Enum.GetValues(typeof(E_SKILL_INDEX))) {
                if(e == E_SKILL_INDEX._Neutral_) continue;
                if(e == E_SKILL_INDEX._Melee_) continue;
                if(e == E_SKILL_INDEX.BlackWhiteHole) continue;
                if(e == E_SKILL_INDEX.ThrowSlash) continue;
                skillEnum.Add(e);
            }
            foreach(var data in ScriptableSkillData)
            {
                string PATH = Path.Combine(Application.dataPath, $"Resources/Json/{skillEnum[index].ToString()}_{(int)skillEnum[index]}.json");
                string json = File.ReadAllText(PATH);
                data.SetSerials(JsonUtility.FromJson<SerialSkillData>(json));
                index++;
            }
        }
    }
}