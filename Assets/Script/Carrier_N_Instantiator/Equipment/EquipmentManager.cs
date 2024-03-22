using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour{
    public List<AbstractEquipment> equipments;    
    //SkillManager
    public Collider playerCollider;
    [HideInInspector] public UnityEvent OnChangeEvent; // -> 이놈을 서브스크라이브 해야된다.

    private void Awake() {
        // ?? 연산은 왼쪽이 null이냐에 따라 아니냐에 따라 값 대입
        // null체크 할빠에 이게 차라리 좀 빠를듯
        equipments ??= new List<AbstractEquipment>(12);
        OnChangeEvent = new UnityEvent();
    }

    public void Equip(AbstractEquipment equipment){
        equipment.InitEquipment(0);
        equipments.Add(equipment);
        OnChangeEvent.Invoke();
        //CalculateEquipmentAddingData();
    }

    public void Unequip(AbstractEquipment equipment){
        equipments.Remove(equipment);
        OnChangeEvent.Invoke();
    }
}