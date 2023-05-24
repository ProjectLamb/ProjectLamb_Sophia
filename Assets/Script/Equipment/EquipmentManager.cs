using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour{
    public List<AbstractEquipment> equipments;
    
    public Player player;
    public Collider playerCollider;

    [SerializeField]
    private MasterData mAddingData;
    public  MasterData AddingData {get; set;}
    
    private void Awake() {
        // ?? 연산은 왼쪽이 null이냐에 따라 아니냐에 따라 값 대입
        // null체크 할빠에 이게 차라리 좀 빠를듯
        equipments ??= new List<AbstractEquipment>(12);
        player ??= GetComponentInParent<Player>();
    }

    /// <summary>
    /// 장비를 장착하거나, 해제하거나, 어떤 버프 이벤트가 들어오면 실행한다.
    /// </summary>
    public void CalculateFinalData(){
        AddingData.Clear();
        foreach(AbstractEquipment E in equipments){
            AddingData += E.equipmentData;
        }
    }

    public void Equip(AbstractEquipment equipment){
        equipment.InitEquipment(player);
        equipments.Add(equipment);
        CalculateFinalData();
    }

    public void Unequip(AbstractEquipment equipment){
        equipments.Remove(equipment);
        CalculateFinalData();
    }
}