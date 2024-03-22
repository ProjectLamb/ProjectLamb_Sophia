using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Composite
{
    using Sophia.DataSystem.Modifiers;
    using Sophia.Entitys;

    public class EquipmentManager : MonoBehaviour
    {
        #region SerializeMember

        [SerializeField] private Entity _entity;
        [SerializeField] private List<string> _currentEquipments;

        #endregion

        public SortedList<string, Equipment> equipedEquipment { get; private set; }
        public IDataAccessible DataAccessible {get; private set;}

        private void Awake()
        {
            equipedEquipment    = new SortedList<string, Equipment>();
            DataAccessible      = _entity;
        }

        public void Equip(Equipment equipment)
        {
            equipedEquipment.Add(equipment.Name, equipment);
            equipedEquipment[equipment.Name].Invoke(DataAccessible);

            _currentEquipments.Clear();
            foreach (var item in equipedEquipment)
            {
                if(item.Value != null){_currentEquipments.Add(item.Key);}
            }
        }

        public void Drop(Equipment equipment)
        {
            equipedEquipment[equipment.Name].Revert(DataAccessible);
            equipedEquipment.Remove(equipment.Name);

            _currentEquipments.Clear();
            foreach (var item in equipedEquipment)
            {
                _currentEquipments.Add(item.Key);
            }
        }

        [ContextMenu("첫번째부터 제거")]
        public void TEST_DropFront()
        {
            if (equipedEquipment.Count != 0)
            {
                equipedEquipment.First().Value.Revert(DataAccessible);
                equipedEquipment.RemoveAt(0);
                _currentEquipments.Clear();
                foreach (var item in equipedEquipment)
                {
                    _currentEquipments.Add(item.Key);
                }
            }

        }
    }
}