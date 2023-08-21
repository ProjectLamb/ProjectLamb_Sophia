using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponInfoUI : MonoBehaviour
{
    public Image WeaponImage;
    public TextMeshProUGUI  MainTextTitle;
    public UITextList       MainTextStatsList;
    public TextMeshProUGUI  DescriptionText;

    public void GetData(Weapon _weapon) {
        WeaponImage.sprite = _weapon.ScriptableWD.WeaponIcon;
        MainTextTitle.text = _weapon.ScriptableWD.WeaponName;
        MainTextStatsList.SetAllTextListByWeapon(_weapon.ScriptableWD);
        DescriptionText.text = _weapon.ScriptableWD.WeaponDescription;
    }
}
