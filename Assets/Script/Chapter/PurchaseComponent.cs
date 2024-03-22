using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sophia_Carriers;

public class PurchaseComponent : MonoBehaviour {
    public UnityAction OnPurchasedEvent;
    public UnityAction OnPurchasedDenyEvent;
    public int price;
    private void Awake() {
        OnPurchasedEvent        += () => {};
        OnPurchasedDenyEvent    += () => {};
    }
    public bool Purchase()
    {
        int current_gear = PlayerDataManager.GetPlayerData().Gear;
        if(current_gear >= this.price){
            PlayerDataManager.GetPlayerData().Gear -= price;
            OnPurchasedEvent.Invoke();
            return true;
        }
        else {
            OnPurchasedDenyEvent.Invoke();
            return false;
        }
    }
    //private void OnTriggerEnter(Collider other) 
    //{
    //    if (!other.TryGetComponent<Player>(out Player player)) { return; }
    //    Purchase();
    //}
}