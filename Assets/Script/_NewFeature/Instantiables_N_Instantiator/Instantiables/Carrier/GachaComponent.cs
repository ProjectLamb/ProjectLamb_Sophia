using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Instantiates
{
    public struct SerialWeightOfItemData
    {
        public float _probsWeightRatio;
        public E_ITEM_TYPE _itemType;
    }
    public class GachaComponent : MonoBehaviour
    {
        public Transform instantPivot;

        [System.Serializable]
        public class GachaItem
        {
            public float probs; //���� Ȯ�� { 1.0f, 14.0f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f };
            public ItemObject item;    //������ ǰ��
            public int count;   //��ȯ ����
            public bool IsLaunch;   //��ȯ�� �� �߻�

            GachaItem()
            {
                count = 1;
            }
        }

        float totalProbs = 100.0f;
        public ItemObjectBucket _bucket;

        public List<GachaItem> gachaItemList;

        public int Gacha()
        {
            System.Random random = new System.Random();
            float randomValue = (float)random.NextDouble() * totalProbs;
            int returnValue = 0;

            float temp = 0.0f;

            for (int i = 0; i < gachaItemList.Count; i++)
            {
                temp += gachaItemList[i].probs;
                if (randomValue <= temp)
                {
                    returnValue = i;
                    break;
                }
            }

            return returnValue;
        }

        public List<ItemObject> InstantiateReward()
        {
            int index = Gacha();
            Debug.Log(index);
            ItemObject item = gachaItemList[index].item;
            List<ItemObject> res = new List<ItemObject>();
            for (int i = 0; i < gachaItemList[index].count; i++)
            {
                if (item == null)
                    return null;

                switch (item)
                {
                    case HealthItemObject:
                        res.Add(_bucket.InstantablePositioning(Instantiate(item)).Init());
                        break;
                    case GearItemObject:
                        res.Add(_bucket.InstantablePositioning(Instantiate(item)).Init());
                        break;
                    case EquipmentItemObject:
                        System.Random random = new System.Random();
                        item = ItemPool.Instance._equipmentItems[random.Next(0, ItemPool.Instance._equipmentItems.Count)];//.GetRandomItem("Equipment").Clone();
                        res.Add(_bucket.InstantablePositioning(Instantiate(item)).Init());
                        break;
                }
            }
            return res;
        }

        private void Awake()
        {
            _bucket ??= GetComponent<ItemObjectBucket>();
            instantPivot = transform;
        }
    }
}