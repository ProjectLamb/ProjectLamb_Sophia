using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using FMODPlus;

namespace Sophia.Instantiates
{
    public class VendingMachine : MonoBehaviour
    {
        Transform pivot;
        public Transform endPos;
        public int price;
        PurchaseComponent pc;
        GachaComponent gc;
        bool IsReady = true;
        Sequence LaunchSequnce;
        System.Random random;

        private void Awake()
        {
            pc = GetComponent<PurchaseComponent>();
            gc = GetComponent<GachaComponent>();
        }
        void Start()
        {
            pc.price = this.price;
            gc.instantPivot = transform.GetChild(0).transform;
            gc._bucket = transform.GetChild(0).GetComponent<ItemObjectBucket>();
            random = new System.Random();
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.transform.TryGetComponent(out Entitys.Player player))
            {
                if (!IsReady)
                    return;
                if (!pc.Purchase(player))
                    return;
                List<ItemObject> positionedItem = gc.InstantiateReward();
                if (positionedItem.Count != 0)
                {
                    foreach (var item in positionedItem)
                    {
                        if(item == null) continue;
                        item.SetTriggerTime(1f).SetTweenSequence(SetSequnce(item)).Activate();
                    }
                }
                AsyncWaitUse(2).Forget();
                GetComponent<FMODAudioSource>().Play();
            }
        }

        public Sequence SetSequnce(ItemObject itemObject)
        {
            Sequence mySequence = DOTween.Sequence();
            System.Random random = new System.Random();
            Vector3 EndPosForward = endPos.transform.right;
            var randomAngle = random.Next(-180, 180);
            Vector3[] rotateMatrix = new Vector3[] {
                new Vector3(Mathf.Cos(randomAngle), 0 , Mathf.Sin(randomAngle)),
                new Vector3(0, 1 , 0),
                new Vector3(-Mathf.Sin(randomAngle), 0 , Mathf.Cos(randomAngle))
            };
            Vector3 retatedVec = Vector3.zero + Vector3.up;
            retatedVec += EndPosForward.x * rotateMatrix[0];
            retatedVec += EndPosForward.y * rotateMatrix[1];
            retatedVec += EndPosForward.z * rotateMatrix[2];
            var randomDist = (float)random.NextDouble() * 7;
            var randomForce = (float)random.NextDouble();
            Debug.Log(retatedVec * randomDist);
            Tween jumpTween = itemObject.transform.DOLocalJump((retatedVec * randomDist) + endPos.transform.position, randomForce * 10, 1, 1).SetEase(Ease.OutBounce);
            return mySequence.Append(jumpTween);
        }

        public async UniTaskVoid AsyncWaitUse(float _waitSecondTime)
        {
            IsReady = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
            IsReady = true;
        }
    }
}