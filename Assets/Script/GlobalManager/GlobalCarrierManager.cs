using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia
{
    using Sophia.Instantiates;
    public class GlobalCarrierManger : MonoBehaviour
    {
        [SerializeField] private List<GearItem> _gearItems;
        [SerializeField] private List<SkillItem> _skillItems;
        [SerializeField] private List<EquipmentItem> _equipmentItems;
        [SerializeField] private HealthItem _healthItem;

        void Awake()
        {
        }

        public Instantiates.Carrier GetRandomItem(string text)
        {
            Carrier tmp = null;
            System.Random random = new System.Random();
            int randomValue = 0;
            switch (text)
            {
                case "Equipment":
                    randomValue = random.Next(0, _equipmentItems.Count);
                    tmp = _equipmentItems[randomValue];
                    _equipmentItems.Remove(tmp as EquipmentItem);
                    break;
                case "Gear":
                    randomValue = random.Next(0, _gearItems.Count);
                    tmp = _gearItems[randomValue];
                    break;
                case "Skill":
                    randomValue = random.Next(0, _skillItems.Count);
                    tmp = _skillItems[randomValue];
                    _skillItems.Remove(tmp as SkillItem);
                    break;
            }
            return tmp;
        }
    }

}