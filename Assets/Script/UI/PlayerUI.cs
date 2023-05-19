using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    public Player Player;
    public Slider hpBar;
    private void Awake() {
        
    }

    private void Update() {
        hpBar.value = Player.playerData.CurHP;
    }
}