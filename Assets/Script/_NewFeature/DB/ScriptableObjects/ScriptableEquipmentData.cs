using System;
using Sophia.Instantiates;
using Sophia.Instantiates.Skills;
using UnityEngine;

namespace Sophia.DB
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "ScriptableObject/Carrier/Item/Equipment", order = int.MaxValue)]
    public class ScriptableEquipmentData : ScriptableObject
    {
        [field : SerializeField] public SerialEquipmentData _serialEquipmentData { get; private set; }

        public void SetSerial(SerialEquipmentData serialEquipmentData)
        {
            _serialEquipmentData = serialEquipmentData;
        }
    }
}