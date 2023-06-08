using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Player player;
    public Slider hpBar;
    private void Start() {
        player.ScriptablePD.HitState += ChangeHPUI;
    }
    public void ChangeHPUI(){
        hpBar.value = player.CurrentHealth;
    }
}