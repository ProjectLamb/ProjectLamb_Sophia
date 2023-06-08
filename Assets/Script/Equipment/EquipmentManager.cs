using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour{
    public List<AbstractEquipment> equipments;    
    //SkillManager
    public Collider playerCollider;
    public UnityEvent OnChangeEvent; // -> 이놈을 서브스크라이브 해야된다.

    private void Awake() {
        // ?? 연산은 왼쪽이 null이냐에 따라 아니냐에 따라 값 대입
        // null체크 할빠에 이게 차라리 좀 빠를듯
        equipments ??= new List<AbstractEquipment>(12);
    }

    /// <summary>
    /// 장비를 장착하거나, 해제하거나, 어떤 버프 이벤트가 들어오면 실행한다.
    /// </summary>
    /// 
    /*
    [ContextMenu("CalculateAddingData")]
    public void CalculateAddingData(){
        AddingData.Clear();
        foreach(AbstractEquipment E in equipments){
            AddingData += E.equipmentData;
        }
        
        AddingData.PipeToPlayer(ref pdTmp);
        AddingData.PipeToWeapon(ref wdTmp);

        player.playerData = player.BasePlayerData + pdTmp;
        player.playerData.EntityTag = "Player";
        weapon.weaponData = weapon.BaseWeaponData + wdTmp;
    }
    */
    
    public void Equip(AbstractEquipment equipment){
        equipment.InitEquipment(0);
        equipments.Add(equipment);
        OnChangeEvent.Invoke();
        //CalculateAddingData();
    }

    public void Unequip(AbstractEquipment equipment){
        equipments.Remove(equipment);
        OnChangeEvent.Invoke();
        //CalculateAddingData();
    }
}