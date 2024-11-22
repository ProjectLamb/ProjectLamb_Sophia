using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Sophia.DB
{
    public class GlobalScriptableEquipmentModelManager : MonoBehaviour
    {
        [SerializedDictionary("EquipmentIndex", "ScriptableEquipmentData")]
        [SerializeField] private SerializedDictionary<int, ScriptableEquipmentData> _scriptableEquipmentDatas;
        
        public SerialEquipmentData GetScriptableEquipmentData(int index)
        {
            return _scriptableEquipmentDatas[index]._serialEquipmentData;
        }
    }
}