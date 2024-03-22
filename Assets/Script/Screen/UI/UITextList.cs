using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AYellowpaper.SerializedCollections;

public class UITextList : MonoBehaviour
{
    public List<(TextMeshProUGUI,TextMeshProUGUI)> StatsTextGroups = new List<(TextMeshProUGUI, TextMeshProUGUI)>();
    public List<string> StatsKeys = new List<string>();

    private void Awake() {
        foreach (Transform item in transform)
        {
            StatsTextGroups.Add((item.GetChild(0).GetComponent<TextMeshProUGUI>(),item.GetChild(1).GetComponent<TextMeshProUGUI>()));
        }
    }

    public void SetAllTextListByStats(){
        IEnumerator AsyncAfterRendered(){
            yield return new WaitForEndOfFrame();
            StatsTextGroups[0].Item1.text = StatsKeys[0];
            StatsTextGroups[1].Item1.text = StatsKeys[1];
            StatsTextGroups[2].Item1.text = StatsKeys[2];
            StatsTextGroups[3].Item1.text = StatsKeys[3];
            StatsTextGroups[4].Item1.text = StatsKeys[4];
            StatsTextGroups[5].Item1.text = StatsKeys[5];
            StatsTextGroups[6].Item1.text = StatsKeys[6];
            StatsTextGroups[7].Item1.text = StatsKeys[7];
            StatsTextGroups[8].Item1.text = StatsKeys[8];

            StatsTextGroups[0].Item2.text = PlayerDataManager.GetEntityData().MaxHP.ToString();
            StatsTextGroups[1].Item2.text = PlayerDataManager.GetEntityData().Power.ToString();
            StatsTextGroups[2].Item2.text = PlayerDataManager.GetEntityData().MoveSpeed.ToString();
            StatsTextGroups[3].Item2.text = PlayerDataManager.GetEntityData().Defence.ToString();
            StatsTextGroups[4].Item2.text = PlayerDataManager.GetEntityData().Tenacity.ToString();
            StatsTextGroups[5].Item2.text = PlayerDataManager.GetEntityData().AttackSpeed.ToString();
            StatsTextGroups[6].Item2.text = PlayerDataManager.GetPlayerData().MaxStamina.ToString();
            StatsTextGroups[7].Item2.text = PlayerDataManager.GetPlayerData().StaminaRestoreRatio.ToString();
            StatsTextGroups[8].Item2.text = PlayerDataManager.GetPlayerData().Luck.ToString();
        }
        StartCoroutine(AsyncAfterRendered());
    }

    public void SetAllTextListByWeapon(ScriptableObjWeaponData _swd){
        IEnumerator AsyncAfterRendered(){
            yield return new WaitForEndOfFrame();
            StatsTextGroups[0].Item1.text = StatsKeys[0];
            StatsTextGroups[0].Item2.text = _swd.DamageRatio.ToString();
            StatsTextGroups[1].Item1.text = StatsKeys[1];
            StatsTextGroups[1].Item2.text = _swd.WeaponDelay.ToString();
            StatsTextGroups[2].Item1.text = StatsKeys[2];
            StatsTextGroups[2].Item2.text = _swd.Range.ToString();
            if(_swd.WeaponType != WEAPON_TYPE.MELEE){
                StatsTextGroups[3].Item1.text = StatsKeys[3];
                StatsTextGroups[3].Item2.text = _swd.Ammo.ToString();
            }
            else {
                StatsTextGroups[3].Item1.text = "";
                StatsTextGroups[3].Item2.text = "";
            }
        }
        StartCoroutine(AsyncAfterRendered());
    }
}