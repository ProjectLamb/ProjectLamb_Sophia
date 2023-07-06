using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_MONEY : MonoBehaviour
{
    public int moneyHack;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataManager.GetPlayerData().Gear = moneyHack;
        GameManager.Instance.PlayerGameObject.GetComponent<Player>().CurrentHealth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
