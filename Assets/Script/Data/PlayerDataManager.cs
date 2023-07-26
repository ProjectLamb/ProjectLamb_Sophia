using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] private Player player; 
    [SerializeField] private WeaponManager weaponManager;
    public  static  PlayerData BasePlayerData;
    public  static  WeaponData BaseWeaponData;
    private static  MasterData EquipmentAddingData;
    private static  MasterData BaseData;
    private static  MasterData FinalData;

    private TextMeshProUGUI debugText;

    [SerializeField]private EquipmentManager equipmentManager;
    private void Awake()
    {
        if (player == null) { throw new System.Exception("Player가 null임 인스펙터 확인 ㄱㄱ"); }
        BasePlayerData = new PlayerData(player.ScriptablePD);

        if (weaponManager == null) { throw new System.Exception("weapon가 null임 인스펙터 확인 ㄱㄱ"); }
        BaseWeaponData = new WeaponData(weaponManager.weapon.ScriptableWD);

        if (equipmentManager == null) { throw new System.Exception("equipmentManager가 null임 인스펙터 확인 ㄱㄱ"); }
        BaseData = new MasterData();
        BaseData += BasePlayerData;
        BaseData += BaseWeaponData;

        EquipmentAddingData = new MasterData();

        FinalData = new MasterData();
        FinalData += (BaseData + EquipmentAddingData);
        
        debugText = GameObject.FindGameObjectWithTag("DebugUI").transform.GetComponentInChildren<TextMeshProUGUI>(true);
    }
    public void CalculateBaseData(){
        BaseData = new MasterData();
        BasePlayerData = new PlayerData(player.ScriptablePD);
        BaseWeaponData = new WeaponData(weaponManager.weapon.ScriptableWD);
        BaseData += BasePlayerData;
        BaseData += BaseWeaponData;
    }
    public void CalculateEquipmentAddingData(){
        /*
            BaseData
                EntityData (버프로 영향이 가지는 수치들) 즉, 이놈들은 초기화를 해도 되긴한데.
                PlayerData (아직까지는 없지만 현재 기획상에서 PlayerData가 일시적으로 변하는것은 없는듯 하다);
            
            그말은 즉슨, 돈이 초기화 되는 오류는
            EntityData만 초기화 해야하는데 PlayerData 부분도 같이 초기화 되서 생기는 문제였다.
            그렇다면 EntityData만 중점적으로 초기화 하도록하고 PlayerData 는 가져와도 될것 같다.
            사실 FinalData에 초기화를 가하는것은 버프, 디버프 같은 일시적인 변화되는 데이터가 

            
        */
        FinalData = BaseData;
        EquipmentAddingData = new MasterData();
        //Adding계산하기
        foreach(AbstractEquipment equipment in equipmentManager.equipments){
            EquipmentAddingData += equipment.AddingData;
        }
        FinalData += EquipmentAddingData;
    }

    private void Update() {
        debugText.text = FinalData.ToString();
    }

    public static void ResetFinal(){
        FinalData = new MasterData();
        FinalData += BaseData;
        FinalData += EquipmentAddingData;
    }

    public static MasterData GetOriginData() {
        MasterData res = new MasterData();
        res += BaseData;
        res += EquipmentAddingData;
        return res;
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