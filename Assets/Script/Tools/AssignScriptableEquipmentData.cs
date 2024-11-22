using System.Collections.Generic;
using Sophia.DB;
using Sophia.Instantiates;
using UnityEngine;

namespace Script.Tools
{
    public class AssignScriptableEquipmentData : MonoBehaviour
    {
        [SerializeField] private List<EquipmentItemObject> ConcreteEquipmentPrefabs;
        [SerializeField] private List<ScriptableEquipmentData> ScriptableEquipmentData;

        [ContextMenu("Assign")]
        public void TEST_ASSIGN()
        {
            int index = 0;
            foreach(var data in ConcreteEquipmentPrefabs) {
                data.Deep_Copy_To_Scriptable(ScriptableEquipmentData[index++]);
            }
        }
    }
}