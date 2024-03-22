using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEditor.Rendering;

[System.Serializable]
public struct MasterData
{
    
    [SerializeField] public PlayerData playerData;
    [SerializeField] public WeaponData weaponData;
    
    public static Sophia.DataSystem.Stat MaxStaminaReferer {get; private set;}
    public static void MaxStaminaInject(Sophia.DataSystem.Stat referPointer) {
        MaxStaminaReferer = referPointer;
    }

    public static MasterData operator +(MasterData x, MasterData y){
        MasterData res = new MasterData();
        res.playerData += x.playerData;
        res.weaponData += x.weaponData;
        res.playerData += y.playerData;
        res.weaponData += y.weaponData;
        return res;
    }

    public static MasterData operator +(MasterData x, PlayerData y){
        MasterData res = new MasterData();
        res.playerData += x.playerData;
        res.weaponData += x.weaponData;
        res.playerData += y;
        return res;
    }

    public static MasterData operator +(MasterData x, WeaponData y){
        MasterData res = new MasterData();
        res.playerData += x.playerData;
        res.weaponData += x.weaponData;
        res.weaponData += y;
        return res;
    }

    public override string ToString()
    {
        string PlayerString = $"PlayerEntityData {playerData.EntityDatas.ToString()}, Luck : {playerData.Luck}, Gear : {playerData.Gear}, Frag : {playerData.Frag} \n";
        string WeaponString = $"DamageRatio : {weaponData.DamageRatio}, WeaponDelay : {weaponData.WeaponDelay}, Range : {weaponData.Range}, Ammo : {weaponData.Ammo}\n";

        string SkillString = "";
        /*
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    SkillString += "Q Key : ";
                    break;
                case 1:
                    SkillString += "E Key : ";
                    break;
                case 2:
                    SkillString += "R Key : ";
                    break;
            }
            for (int j = 0; j < 3; j++)
            {
                SkillString += $"NumericArray : {SkillRankInfos[i].numericArray[j]}, ";
                SkillString += $"SkillDelay : {SkillRankInfos[i].skillDelay[j]}, ";
                SkillString += $"DurateTime : {SkillRankInfos[i].durateTime[j]}\n";
            }
        }
        */
        return (PlayerString + WeaponString + SkillString);
    }

}