using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EquipmentInfoUI : MonoBehaviour
{
    public UISliderList     SliderList;
    private void OnEnable() {
        GetData();
    }
    public void GetData() {
        SliderList.SetAllTextListByStats();
    }
}
