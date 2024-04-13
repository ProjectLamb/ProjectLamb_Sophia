using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia
{
    using Sophia.Instantiates;
    public enum E_ITEM_TYPE
    {
        None, Gear, Skill, Equipment, Health
    }

    public enum E_EQUIPMENT_TYPE
    {
        None, Normal, Shop, Hidden, Boss
    }
    public class ItemPool : MonoBehaviour
    {
        private int[] equipmentStartIndex = new int[5] { 0, 9999, 9999, 9999, 9999 };
        private int[] equipmentEndIndex = new int[5] { 0, -1, -1, -1, -1 };

        private static ItemPool _instance;

        public static ItemPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType(typeof(ItemPool)) as ItemPool;
                    if (_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set { }
        }

        [SerializeField] public List<GearItemObject> _gearItems;
        [SerializeField] public List<SkillItemObject> _skillItems;
        [SerializeField] public List<EquipmentItemObject> _equipmentItems;
        [SerializeField] public HealthItemObject _healthItem;

        void Awake()
        {
            SetEquipmentIndexFromList();
        }

        void SetEquipmentIndexFromList()
        {
            for (int i = 0; i < _equipmentItems.Count; i++)
            {
                E_EQUIPMENT_TYPE type = E_EQUIPMENT_TYPE.None;
                //Normal
                if (_equipmentItems[i].GetSerialEquipmentData()._equipmentID >= 1000 && _equipmentItems[i].GetSerialEquipmentData()._equipmentID < 2000)
                {
                    type = E_EQUIPMENT_TYPE.Normal;
                }
                //Shop
                else if (_equipmentItems[i].GetSerialEquipmentData()._equipmentID >= 2000 && _equipmentItems[i].GetSerialEquipmentData()._equipmentID < 3000)
                {
                    type = E_EQUIPMENT_TYPE.Shop;
                }
                //Hidden
                else if (_equipmentItems[i].GetSerialEquipmentData()._equipmentID >= 3000 && _equipmentItems[i].GetSerialEquipmentData()._equipmentID < 4000)
                {
                    type = E_EQUIPMENT_TYPE.Hidden;
                }
                //Boss
                else if (_equipmentItems[i].GetSerialEquipmentData()._equipmentID >= 4000 && _equipmentItems[i].GetSerialEquipmentData()._equipmentID < 5000)
                {
                    type = E_EQUIPMENT_TYPE.Boss;
                }

                if (i < equipmentStartIndex[(int)type])
                {
                    equipmentStartIndex[(int)type] = i;
                }
                if (i > equipmentEndIndex[(int)type])
                {
                    equipmentEndIndex[(int)type] = i;
                }
            }
        }

        public EquipmentItemObject GetRandomEquipment(E_EQUIPMENT_TYPE type)
        {
            System.Random random = new System.Random();
            EquipmentItemObject equipmentItemObject = null;

            equipmentItemObject = _equipmentItems[random.Next(equipmentStartIndex[(int)type], equipmentEndIndex[(int)type] + 1)];

            return equipmentItemObject;
        }

    }

}