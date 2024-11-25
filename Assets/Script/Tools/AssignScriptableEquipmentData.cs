using System.Collections.Generic;
using System.IO;
using Sophia.DB;
using Sophia.Instantiates;
using UnityEngine;
using Sophia;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using Cysharp.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Script.Tools
{
    public class AssignScriptableEquipmentData : MonoBehaviour
    {
        [SerializeField] private List<EquipmentItemObject> ConcreteEquipmentPrefabs;
        [SerializeField] private List<ScriptableEquipmentData> ScriptableEquipmentData;
        [SerializeField] private List<int> EquipmentIDs;
        
        const string AddressablePATH = "Scriptable/Data/Item/Equipment";

        [ContextMenu("Assign")]
        public void TEST_ASSIGN()
        {
            int index = 0;
            foreach(var data in ConcreteEquipmentPrefabs) {
                // data.Deep_Copy_To_Scriptable(ScriptableEquipmentData[index++]);
            }
        }
        
        [ContextMenu("Overlap")]
        public async void TEST_OVERLAP()
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
                
                // SerializedObject so = new SerializedObject(data);
                string JSONPATH = Path.Combine(Application.dataPath, $"Resources/Json/{eT.ToString()}_{EquipmentIDs[index].ToString("0000")}.json");
                var SO = await Addressables.LoadAssetAsync<ScriptableEquipmentData>(AddressablePATH +
                    $"/{eT.ToString()}_{EquipmentIDs[index].ToString("0000")}.asset");
                string json = File.ReadAllText(JSONPATH);
                data.SetSerials(JsonUtility.FromJson<SerialEquipmentData>(json));
                EditorUtility.SetDirty(SO);
                AssetDatabase.SaveAssetIfDirty(SO);
                // so.ApplyModifiedProperties();
                index++;
            }
        }
    }
}