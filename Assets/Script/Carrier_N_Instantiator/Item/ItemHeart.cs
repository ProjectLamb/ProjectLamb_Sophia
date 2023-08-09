using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;
public class ItemHeart : Purchase
{
    int recoveryValue;
    public override Carrier Clone()
    {
        if (this.IsCloned == true) throw new SystemException("복제본이 복제본을 만들 수 는 없다.");
        Carrier res = Instantiate(this);
        res.DisableSelf();
        res.IsCloned = true;
        return res;
    }

    public override void Init(Entity _ownerEntity)
    {
        if (IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
        IsInitialized = true;
    }

    public override void InitByObject(Entity _ownerEntity, object[] _objects)
    {
        if (IsCloned == false) throw new System.Exception("원본데이터를 조작하고 있음");
        recoveryValue = (int)_objects[0];
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player player)) { return; }
        if (player.CurrentHealth >= player.GetFinalData().MaxHP)
            return;
        if (IsShopItem)
        {
            if (!purchase(price))
                return;
            GameManager.Instance.Shop.GetComponent<Shop>().HeartCount++;
            GameManager.Instance.Shop.GetComponent<Shop>().InstantiateItem(2, 1);
        }
        GameManager.Instance.PlayerGameObject.GetComponent<Player>().CurrentHealth += recoveryValue;
        DestroySelf();
    }

    protected override void Awake()
    {
        IsShopItem = false;
    }
}