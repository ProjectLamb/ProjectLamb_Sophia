using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public PlayerData playerData;
    public Slider hpBar;

    private void Update() {
        hpBar.value = playerData.numericData.CurHP;
    }
}