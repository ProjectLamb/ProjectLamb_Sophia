using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using Sophia.Instantiates;

public class VendingMachine : MonoBehaviour
{
    Transform pivot;
    public int price;
    PurchaseComponent pc;
    GachaComponent gc;
    bool IsReady = true;

    private void Awake()
    {
        pc = GetComponent<PurchaseComponent>();
        gc = GetComponent<GachaComponent>();
    }
    void Start()
    {
        pc.price = this.price;
        gc.instantPivot = transform.GetChild(0).transform;
        gc.carrierBucket = transform.GetChild(0).GetComponent<CarrierBucket>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject != GameManager.Instance.PlayerGameObject) { return; }
        if (!IsReady)
            return;
        if (!pc.Purchase())
            return;
        gc.InstantiateReward(gc.instantPivot);
        AsyncWaitUse(2).Forget();
    }

    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime)
    {
        IsReady = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        IsReady = true;
    }
}