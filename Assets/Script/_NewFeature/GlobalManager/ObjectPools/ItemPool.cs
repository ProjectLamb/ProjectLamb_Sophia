using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia
{
    using AYellowpaper.SerializedCollections;
    using Sophia.DataSystem.Modifiers;
    using Sophia.Instantiates;
    using Sophia.Instantiates.Skills;

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

        [SerializedDictionary("Key", "Equipments")]
        [SerializeField] public SerializedDictionary<E_EQUIPMENT_TYPE, List<EquipmentItemObject>> _equipmentItemsDic;
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

            equipmentItemObject = _equipmentItemsDic[type][random.Next(0, _equipmentItemsDic[type].Count)];
            int EquipmentId = equipmentItemObject.GetSerialEquipmentData()._equipmentID;
            if(1000 <= EquipmentId && EquipmentId < 2000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Remove(equipmentItemObject);
            }
            else if(2000 <= EquipmentId && EquipmentId < 3000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Remove(equipmentItemObject);
            }
            else if(3000 <= EquipmentId && EquipmentId < 4000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Hidden].Remove(equipmentItemObject);
            }
            else if(4000 <= EquipmentId && EquipmentId < 5000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Boss].Remove(equipmentItemObject);
            }
            _equipmentItems.Remove(equipmentItemObject);
            return equipmentItemObject;
        }

        public SkillItemObject GetRandomSkill() {
            System.Random random = new System.Random();
            SkillItemObject skillItemObject = null;
            skillItemObject = _skillItems[random.Next(0, ItemPool.Instance._skillItems.Count)];
            
            _skillItems.Remove(skillItemObject);

            return skillItemObject;
        }
        
        public EquipmentItemObject GetRandomEquipment() {
            System.Random random = new System.Random();
            EquipmentItemObject equipmentItemObject = null;
            equipmentItemObject = _equipmentItems[random.Next(0, ItemPool.Instance._equipmentItems.Count)];
            int EquipmentId = equipmentItemObject.GetSerialEquipmentData()._equipmentID;
            if(1000 <= EquipmentId && EquipmentId < 2000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Remove(equipmentItemObject);
            }
            else if(2000 <= EquipmentId && EquipmentId < 3000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Remove(equipmentItemObject);
            }
            else if(3000 <= EquipmentId && EquipmentId < 4000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Hidden].Remove(equipmentItemObject);
            }
            else if(4000 <= EquipmentId && EquipmentId < 5000) {
                _equipmentItemsDic[E_EQUIPMENT_TYPE.Boss].Remove(equipmentItemObject);
            }
            _equipmentItems.Remove(equipmentItemObject);
            return equipmentItemObject;
        }

        public EquipmentItemObject GetRandomShopEquipment() {
            System.Random random = new System.Random();
            EquipmentItemObject equipmentItemObject = null;
            int EquipmentType = random.Next(1,12392) % 2;
            if(EquipmentType == 0) {
                if(_equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Count > 0) {
                    equipmentItemObject = _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal][random.Next(0, _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Count)];
                    _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Remove(equipmentItemObject);
                    _equipmentItems.Remove(equipmentItemObject);
                }
                else if(_equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Count > 0) {
                    equipmentItemObject = _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop][random.Next(0, _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Count)];
                    _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Remove(equipmentItemObject);
                    _equipmentItems.Remove(equipmentItemObject);
                }
                else {
                    return null;
                }
            }
            else {
                if(_equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Count > 0) {
                    equipmentItemObject = _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop][random.Next(0, _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Count)];
                    _equipmentItemsDic[E_EQUIPMENT_TYPE.Shop].Remove(equipmentItemObject);
                    _equipmentItems.Remove(equipmentItemObject);
                }
                else if(_equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Count > 0) {
                    equipmentItemObject = _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal][random.Next(0, _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Count)];
                    _equipmentItemsDic[E_EQUIPMENT_TYPE.Normal].Remove(equipmentItemObject);
                    _equipmentItems.Remove(equipmentItemObject);
                }
                else {
                    return null;
                }
            }
            return equipmentItemObject;
        }
    }

}