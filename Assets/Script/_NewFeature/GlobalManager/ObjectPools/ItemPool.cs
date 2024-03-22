using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia
{
    using Sophia.Instantiates;
    public enum E_ITEM_TYPE {
        None, Gear, Skill, Equipment, Health
    }
    public class ItemPool : MonoBehaviour
    {
        private static ItemPool _instance;

        public static ItemPool Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(ItemPool)) as ItemPool;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set {}
        }

        [SerializeField] public List<GearItemObject>         _gearItems;
        [SerializeField] public List<SkillItemObject>        _skillItems;
        [SerializeField] public List<EquipmentItemObject>    _equipmentItems;
        [SerializeField] public HealthItemObject             _healthItem;

    }

}