using UnityEngine;

namespace Sophia.DB
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "ScriptableObject/Carrier/Item/Equipment", order = int.MaxValue)]
    public class ScriptableEquipmentData : ScriptableObject
    {
        [SerializeField] public SerialEquipmentData _serialEquipmentData;
    }
}