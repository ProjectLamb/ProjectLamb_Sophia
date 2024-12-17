using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Sophia.DB
{
    public class GlobalScriptableEquipmentModelManager : MonoBehaviour
    {
        [SerializedDictionary("EquipmentIndex", "ScriptableEquipmentData")] [field: SerializeField]
        private SerializedDictionary<int, ScriptableEquipmentData> _scriptableEquipmentDatas;
        public IReadOnlyDictionary<int, ScriptableEquipmentData> ScriptableEquipmentDatas => _scriptableEquipmentDatas;
    }
}