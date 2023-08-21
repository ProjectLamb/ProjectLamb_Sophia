using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;

public class VendingMachine : MonoBehaviour
{
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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != GameManager.Instance.PlayerGameObject) { return; }
        if (!IsReady)
            return;
        if (!pc.Purchase())
            return;
        gc.instantPivot.position = transform.GetChild(0).transform.position;
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