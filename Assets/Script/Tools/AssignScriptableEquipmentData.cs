using System.Collections.Generic;
using System.IO;
using Sophia.DB;
using Sophia.Instantiates;
using UnityEngine;
using Sophia;

namespace Script.Tools
{
    public class AssignScriptableEquipmentData : MonoBehaviour
    {
        [SerializeField] private List<EquipmentItemObject> ConcreteEquipmentPrefabs;
        [SerializeField] private List<ScriptableEquipmentData> ScriptableEquipmentData;
        [SerializeField] private List<int> EquipmentIDs;

        [ContextMenu("Assign")]
        public void TEST_ASSIGN()
        {
            int index = 0;
            foreach(var data in ConcreteEquipmentPrefabs) {
                // data.Deep_Copy_To_Scriptable(ScriptableEquipmentData[index++]);
            }
        }
        
        [ContextMenu("Overlap")]
        public void TEST_OVERLAP()
        {
            int index = 0;
            foreach(var data in ScriptableEquipmentData)
            {
                var eT = E_EQUIPMENT_TYPE.None;
                if(1000 <= EquipmentIDs[index] && EquipmentIDs[index] < 2000) {
                    eT = E_EQUIPMENT_TYPE.Normal;
                }
                else if(2000 <= EquipmentIDs[index] && EquipmentIDs[index] < 3000) {
                    eT = E_EQUIPMENT_TYPE.Shop;
                }
                else if(3000 <= EquipmentIDs[index] && EquipmentIDs[index] < 4000) {
                    eT = E_EQUIPMENT_TYPE.Hidden;
                }
                else if(4000 <= EquipmentIDs[index] && EquipmentIDs[index] < 5000) {
                    eT = E_EQUIPMENT_TYPE.Boss;
                }
                
                string PATH = Path.Combine(Application.dataPath, $"Resources/Json/{eT.ToString()}_{EquipmentIDs[index]}.json");
                string json = File.ReadAllText(PATH);
                data.SetSerials(JsonUtility.FromJson<SerialEquipmentData>(json));
                index++;
            }
        }
    }
}