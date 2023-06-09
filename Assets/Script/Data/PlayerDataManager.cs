using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class PlayerDataManager : MonoBehaviour
{
    [SerializeField]private Player player; 
    [SerializeField]private Weapon weapon; 
    public static PlayerData BasePlayerData;
    public static WeaponData BaseWeaponData;
    private static MasterData EquipmentAddingData;
    private static MasterData BaseData;
    private static MasterData FinalData;

    private TextMeshProUGUI debugText;

    [SerializeField]private EquipmentManager equipmentManager;
    private void Awake()
    {
        if (player == null) { throw new System.Exception("Player가 null임 인스펙터 확인 ㄱㄱ"); }
        BasePlayerData = new PlayerData(player.ScriptablePD);

        if (weapon == null) { throw new System.Exception("weapon가 null임 인스펙터 확인 ㄱㄱ"); }
        BaseWeaponData = new WeaponData(weapon.ScriptableWD);

        if (equipmentManager == null) { throw new System.Exception("equipmentManager가 null임 인스펙터 확인 ㄱㄱ"); }
        BaseData = new MasterData();
        BaseData += BasePlayerData;
        BaseData += BaseWeaponData;
        EquipmentAddingData = new MasterData();

        FinalData = new MasterData();
        FinalData += BaseData + EquipmentAddingData;
        debugText = GameObject.FindGameObjectWithTag("DebugUI").transform.GetComponentInChildren<TextMeshProUGUI>(true);
    }
    public void CalculateEquipmentAddingData(){
        FinalData = BaseData;
        EquipmentAddingData.Clear();
        //Adding계산하기
        foreach(AbstractEquipment E in equipmentManager.equipments){
            EquipmentAddingData += E.EquipmentData;
        }
        FinalData += EquipmentAddingData;
    }

    private void Update() {
        debugText.text = FinalData.ToString();
    }

    public static void ResetFinal(){
        FinalData = BaseData + EquipmentAddingData;
    }

    public static MasterData GetOriginData() {
        return BaseData + EquipmentAddingData;
    }
    public static ref EntityData GetEntityData(){
        return ref FinalData.playerData.EntityDatas;
    }
    public static ref MasterData GetFinalData(){
        return ref FinalData;
    }
    public static ref PlayerData GetPlayerData(){
        return ref FinalData.playerData;
    }
    public static ref WeaponData GetWeaonData(){
        return ref FinalData.weaponData;
    }
}