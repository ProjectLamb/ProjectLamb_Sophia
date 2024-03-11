using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class GachaComponent : MonoBehaviour
{
    public Transform instantPivot;

    [System.Serializable]
    public class GachaItem
    {
        public float probs; //���� Ȯ�� { 1.0f, 14.0f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f };
        public Carrier item;    //������ ǰ��
        public int count;   //��ȯ ����
        public bool IsLaunch;   //��ȯ�� �� �߻�

        GachaItem()
        {
            count = 1;
        }
    }

    float totalProbs = 100.0f;
    public CarrierBucket carrierBucket;

    public List<GachaItem> gachaItemList;

    public int Gacha()
    {
        System.Random random = new System.Random();
        int returnValue = 0;
        float randomValue = (float)random.NextDouble() * totalProbs;

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

    void InstantiateItem(int index)
    {
        if (gachaItemList[index].item == null)
            return;

        Carrier carrier = gachaItemList[index].item;
        switch (gachaItemList[index].item)
        {
            case ItemHeart:
                carrier = carrier.Clone();
                carrier.InitByObject(null, new object[] { 30 });
                break;
            case ItemGear:
                carrier = carrier.Clone();
                carrier.Init(null);
                break;
            case ItemEquipment:
                //carrier = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
                carrier.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                break;
        }
        carrierBucket.CarrierTransformPositionning(gameObject, carrier);
    }

    public void InstantiateReward(Transform pivot)
    {
        int index = Gacha();
        Debug.Log(index);
        for (int i = 0; i < gachaItemList[index].count; i++)
        {
            if (gachaItemList[index].IsLaunch)
            {
                InstantiateItem(index);
                //위로 쏴야하니 AddForce 필요
            }
            else
            {
                InstantiateItem(index);
                //��ȯ �ݺ�.pivot.position + 랜덤 값
            }
        }
    }

    private void Awake()
    {
        carrierBucket = GetComponent<CarrierBucket>();
        instantPivot = transform;
    }
}
