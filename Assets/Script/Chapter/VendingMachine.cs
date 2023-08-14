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
        Transform instantiatePivot;
        public CarrierBucket carrierBucket;
        float[] probs;
        float totalProbs = 100.0f;
        bool IsReady = true;
        // Start is called before the first frame update
        void Start()
        {
            carrierBucket = GetComponentInChildren<CarrierBucket>();
            instantiatePivot = transform.GetChild(0).transform;
            probs = new float[7] { 2.5f, 7f, 3.5f, 7f, 20.0f, 30f, 30f };
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
                int count = 1;
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
                        Debug.Log("50원");
                        count = 10;
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                        tmp.Init(null);
                        break;
                    case 3:
                        Debug.Log("25원");
                        count = 5;
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                        tmp.Init(null);
                        break;
                    case 4:
                        Debug.Log("10원");
                        count = 2;
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                        tmp.Init(null);
                        break;
                    case 5:
                        Debug.Log("5원");
                        tmp = GameManager.Instance.GlobalCarrierManager.GearList[(int)GEAR_TYPE.GOLD].Clone();
                        tmp.Init(null);
                        break;
                    case 6:
                        Debug.Log("꽝");
                        break;
                }
                for(int i = 0; i < count; i++)
                    carrierBucket.CarrierTransformPositionning(instantiatePivot.gameObject, tmp);
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