using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ItemEquipment : Carrier {
//  public E_CarrierType carrierType;
//  public    VFXObject destroyEffect = null;
//  protected Collider  carrierCollider = null;
//  protected Rigidbody carrierRigidBody = null;
//  protected bool      isInitialized = false;
    public AbstractEquipment equipment;

    protected override void Awake() {
        base.Awake();
        this.carrierType = E_CarrierType.Item;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(!other.TryGetComponent<Player>(out Player player)){return;}
        AbstractEquipment triggerdEquipment = other.GetComponent<AbstractEquipment>();
        player.equipmentManager.Equip(this.equipment);
        Debug.Log($"장비 장착! : {equipment.equipmentName}");
        DestroySelf();
    }
}