using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using Sophia_Carriers;

public class RerollMachine : MonoBehaviour
{
    GameObject shop;
    bool IsReady = true;
    void Awake()
    {
        shop = transform.parent.gameObject;
    }
    private void OnCollisionEnter(Collision other)
    {
        /*if (other.gameObject != GameManager.Instance.PlayerGameObject) { return; }
        if (!IsReady)
            return;
        if (!purchase(price))
            return;
        for (int i = 0; i < 2; i++)
        {
            if (shop.GetComponent<Shop>().ItemArray[i] != null)
            {
                // shop.GetComponent<Shop>().ItemArray[i].DestroySelf();
                // shop.GetComponent<Shop>().InstantiateItem(i, 0);
            }
        }*/
        AsyncWaitUse(2).Forget();
    }
    public async UniTaskVoid AsyncWaitUse(float _waitSecondTime)
    {
        IsReady = false;
        await UniTask.Delay(TimeSpan.FromSeconds(_waitSecondTime));
        IsReady = true;
    }
}
