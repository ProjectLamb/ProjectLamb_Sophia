using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class GlobalCarrierManger : MonoBehaviour
{
    public List<ItemEquipment> EquipmentList;
    public List<ItemGear> GearList;
    public List<ItemSkill> SkillList;
    public ItemHeart itemHeart;
    
    void Awake()
    {
        EquipmentList ??= new List<ItemEquipment>();
        GearList ??= new List<ItemGear>();
        SkillList ??= new List<ItemSkill>();
    }

    public Carrier GetRandomItem(string text)
    {
        Carrier tmp = null;
        System.Random random = new System.Random();
        int randomValue = 0;
        switch (text)
        {
            case "Equipment":
                randomValue = random.Next(0, EquipmentList.Count);
                tmp = EquipmentList[randomValue];
                break;
            case "Gear":
                randomValue = random.Next(0, GearList.Count);
                tmp = GearList[randomValue];
                break;
            case "Skill":
                randomValue = random.Next(0, SkillList.Count);
                tmp = SkillList[randomValue];
                break;
        }
        return tmp;
    }

}
