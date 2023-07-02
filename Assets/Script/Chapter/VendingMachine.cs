using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace Sophia_Carriers
{
    public class VendingMachine : Purchase
    {
        public CarrierBucket carrierBucket;
        float[] probs;
        float totalProbs = 100.0f;
        public int price;
        bool IsReady = true;
        // Start is called before the first frame update
        void Start()
        {
            carrierBucket = GetComponentInChildren<CarrierBucket>();
            probs = new float[7] { 5.0f, 10.0f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f };
        }

        int Gacha()
        {
            int returnValue = 0;
            float randomValue = Random.value * totalProbs;

            float temp = 0.0f;

            for (int i = 0; i < 7; i++)
            {
                temp += probs[i];
                if (randomValue <= temp)
                {
                    returnValue = i;
                    break;
                }
            }

            return returnValue;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject != GameManager.Instance.PlayerGameObject) { return; }
            if (!IsReady)
                return;
            if (purchase(price))
            {
                Carrier tmp = null;
                switch (Gacha())
                {
                    case 0:
                        Debug.Log("부품");
                        tmp = GameManager.Instance.GlobalCarrierManager.GetRandomItem("Equipment").Clone();
                        tmp.Init(GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                        break;
                    case 1:
                        Debug.Log("체력");
                        tmp = GameManager.Instance.GlobalCarrierManager.itemHeart.Clone();
                        tmp.InitByObject(null, new object[]{30});
                        break;
                    case 2:
                        Debug.Log("30원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.DIAMOND].Clone();
                        tmp.Init(null);
                        break;
                    case 3:
                        Debug.Log("20원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.PLATINUM].Clone();
                        tmp.Init(null);
                        break;
                    case 4:
                        Debug.Log("10원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                        tmp.Init(null);
                        break;
                    case 5:
                        Debug.Log("5원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.SILVER].Clone();
                        tmp.Init(null);
                        break;
                    case 6:
                        Debug.Log("1원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.BRONZE].Clone();
                        tmp.Init(null);
                        break;
                }
                carrierBucket.CarrierTransformPositionning(gameObject, tmp);
            }
            else
            {
                Debug.Log("돈이 없다.");
            }
            AsyncWaitUse(2).Forget();
        }

        public async UniTaskVoid AsyncWaitUse(float _waitSecondTime)
        {
            IsReady = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
            IsReady = true;
        }
    }
}