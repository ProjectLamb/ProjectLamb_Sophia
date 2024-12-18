using System;
using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
using UnityEngine;

namespace Sophia.DB
{
    public interface IEquipmentDataAccessable
    {
        public int EquipmentID { get; }
        public string EquipmentName { get; }
        public string EquipmentDescription { get; }
        public Sprite EquipmentIcon { get; }
        public SerialStatCalculateDatas EquipmentStat { get; }
        public SerialExtrasCalculateDatas EquipmentStatExtras { get; }
    }
    
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "ScriptableObject/Carrier/Item/Equipment", order = int.MaxValue)]
    public class ScriptableEquipmentData : ScriptableObject, IEquipmentDataAccessable
    {
        [SerializeField] private SerialEquipmentData _serialEquipmentData;

        public int EquipmentID => _serialEquipmentData._equipmentID;
        public string EquipmentName => _serialEquipmentData._equipmentName;
        public string EquipmentDescription => _serialEquipmentData._description;
        public Sprite EquipmentIcon => _serialEquipmentData._icon;
        public SerialStatCalculateDatas EquipmentStat => _serialEquipmentData._statCalculateDatas;
        public SerialExtrasCalculateDatas EquipmentStatExtras => _serialEquipmentData._extrasCalculateDatas;
    }
}