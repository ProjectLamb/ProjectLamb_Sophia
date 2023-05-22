using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour{
    public Player player;
    public List<AbstractEquipment> equipments;
    public Collider playerCollider;
    private void Awake() {
        if(equipments == null) {equipments = new List<AbstractEquipment>(12);}
        if(player == null) player = GetComponentInParent<Player>();
    }
    private void Start() {
        foreach(AbstractEquipment E in equipments){
            E.InitEquipment();
        }
    }

    public void CalculatePipeline(){
        
    }

    [ContextMenu("모두 장착")]
    public void EquipAll(){
        foreach(AbstractEquipment E in equipments){
            E.Equip(player, 0);
        }
    }
    [ContextMenu("모두 해제")]
    public void UnequipAll(){
        foreach(AbstractEquipment E in equipments){
            E.Unequip(player, 0);
        }  
    }

    [ContextMenu("1번 인덱스로 장착")]
    public void EquipAll_1(){
        foreach(AbstractEquipment E in equipments){
            E.Equip(player, 1);
        }
    }
    [ContextMenu("1번 인덱스로 모두 해제")]
    public void UnequipAll_1(){
        foreach(AbstractEquipment E in equipments){
            E.Unequip(player, 1);
        }  
    }
}