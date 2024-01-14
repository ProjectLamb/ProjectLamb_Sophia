using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusInfoUI : MonoBehaviour
{
    public TextMeshProUGUI  NameText;
    public TextMeshProUGUI  AgeText;
    public TextMeshProUGUI  StatusText;
    public UITextList       StatsList;
    private void OnEnable() {
        GetData();
    }
    public void GetData() {
        NameText.text = "_NameText_";
        AgeText.text = "_AgeText_";
        StatusText.text = "Stauts";
        StatsList.SetAllTextListByStats();
    }
}
