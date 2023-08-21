using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AYellowpaper.SerializedCollections;
using DG.Tweening;

public class UISliderList : MonoBehaviour
{
    public List<(TextMeshProUGUI, Slider)> StatsTextGroups = new List<(TextMeshProUGUI, Slider)>();
    public List<string> StatsKeys = new List<string>();
    public List<float>  SliderMaxValue = new List<float>();

    private void Awake() {
        foreach (Transform item in transform)
        {
            StatsTextGroups.Add((
                item.GetChild(1).GetComponent<TextMeshProUGUI>(),
                item.GetChild(0).GetComponent<Slider>()
            ));
            StatsTextGroups.Last().Item2.maxValue =  SliderMaxValue[StatsTextGroups.Count-1];
        }
    }

    private void OnEnable() {
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

            yield return new WaitForEndOfFrame();
            // Debug.Log("Start");
            StatsTextGroups[0].Item2.value = (float)PlayerDataManager.GetEntityData().MaxHP;
            StatsTextGroups[1].Item2.value = (float)PlayerDataManager.GetEntityData().MoveSpeed;
            StatsTextGroups[2].Item2.value = (float)PlayerDataManager.GetEntityData().Defence;
            StatsTextGroups[3].Item2.value = (float)PlayerDataManager.GetEntityData().Tenacity;
            StatsTextGroups[4].Item2.value = (float)PlayerDataManager.GetEntityData().Power;
            StatsTextGroups[5].Item2.value = (float)PlayerDataManager.GetEntityData().AttackSpeed;
            StatsTextGroups[6].Item2.value = (float)PlayerDataManager.GetPlayerData().MaxStamina;
            // StatsTextGroups[0].Item2.value = 0; StatsTextGroups[0].Item2.DOValue((float)PlayerDataManager.GetEntityData().MaxHP, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[1].Item2.value = 0; StatsTextGroups[1].Item2.DOValue((float)PlayerDataManager.GetEntityData().MoveSpeed, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[2].Item2.value = 0; StatsTextGroups[2].Item2.DOValue((float)PlayerDataManager.GetEntityData().Defence, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[3].Item2.value = 0; StatsTextGroups[3].Item2.DOValue((float)PlayerDataManager.GetEntityData().Tenacity, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[4].Item2.value = 0; StatsTextGroups[4].Item2.DOValue((float)PlayerDataManager.GetEntityData().Power, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[5].Item2.value = 0; StatsTextGroups[5].Item2.DOValue((float)PlayerDataManager.GetEntityData().AttackSpeed, 1f).SetEase(Ease.InCubic).Play();
            // StatsTextGroups[6].Item2.value = 0; StatsTextGroups[6].Item2.DOValue((float)PlayerDataManager.GetPlayerData().MaxStamina, 1f).SetEase(Ease.InCubic).Play();
        }
        StartCoroutine(AsyncAfterRendered());
    }
}