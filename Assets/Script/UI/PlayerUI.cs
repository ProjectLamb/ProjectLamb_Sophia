using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Player player;
    public Slider hpBar;
    private void Awake() {
        
    }

    private void Update() {
        hpBar.value = player.playerData.CurHP;
    }
}