using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerDataManager : MonoBehaviour
{
    private static MasterData AddingData;
    [SerializeField]private Player player; 
    public static PlayerData BasePlayerData;
    [SerializeField]private Weapon weapon; 
    public static WeaponData BaseWeaponData;
    
    [SerializeField]private EquipmentManager equipmentManager;
    private void Awake()
    {
        if (player == null) { throw new System.Exception("Player가 null임 인스펙터 확인 ㄱㄱ"); }
        BasePlayerData = new PlayerData(player.ScriptablePD);
        if (weapon == null) { throw new System.Exception("weapon가 null임 인스펙터 확인 ㄱㄱ"); }
        BaseWeaponData = new WeaponData(weapon.ScriptableWD);
        if (equipmentManager == null) { throw new System.Exception("equipmentManager가 null임 인스펙터 확인 ㄱㄱ"); }
        AddingData = new MasterData();
        AddingData += BasePlayerData;
        AddingData += BaseWeaponData;
    }

    public void ResetBaseData(){
        BasePlayerData = new PlayerData(player.ScriptablePD);
        BaseWeaponData = new WeaponData(weapon.ScriptableWD);
    }

    public void CalculateData(){
        AddingData.Clear();
        //Base만들기
        AddingData += BasePlayerData;
        AddingData += BaseWeaponData;
        //Adding계산하기
        foreach(AbstractEquipment E in equipmentManager.equipments){
            AddingData += E.equipmentData;
        }
    }
    public static ref EntityData GetEntityData(){
        return ref AddingData.playerData.EntityDatas;
    }
    public static ref MasterData GetAddingData(){
        return ref AddingData;
    }
    public static ref WeaponData GetWeaonData(){
        return ref AddingData.weaponData;
    }
    public static ref PlayerData GetPlayerData(){
        return ref AddingData.playerData;
    }
}