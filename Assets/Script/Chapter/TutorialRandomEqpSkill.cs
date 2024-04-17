using System.Collections;
using System.Collections.Generic;
using Sophia;
using Sophia.Instantiates;
using UnityEngine;

public class TutorialRandomEqpSkill : MonoBehaviour
{
    ItemObjectBucket[] itemObjectBucket = new ItemObjectBucket[2];

    void Awake()
    {
        itemObjectBucket[0] = transform.GetChild(0).GetComponent<ItemObjectBucket>();
        itemObjectBucket[1] = transform.GetChild(1).GetComponent<ItemObjectBucket>();
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomEqpSkill();
    }

    void RandomEqpSkill()
    {
        ItemObject itemObject = null;

        //Equipment
        itemObject = ItemPool.Instance.GetRandomEquipment(E_EQUIPMENT_TYPE.Normal);
        itemObjectBucket[0].InstantablePositioning(itemObject = Instantiate(itemObject).Init()).Activate();
        itemObject.transform.parent = transform.GetChild(0);

        //Skill
        System.Random random = new System.Random();
        itemObject = ItemPool.Instance._skillItems[random.Next(0, ItemPool.Instance._skillItems.Count)];
        itemObjectBucket[1].InstantablePositioning(itemObject = Instantiate(itemObject).Init()).Activate();
        itemObject.transform.parent = transform.GetChild(1);
    }
}
