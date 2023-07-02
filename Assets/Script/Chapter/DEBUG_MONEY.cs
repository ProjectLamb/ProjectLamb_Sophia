using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_MONEY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataManager.GetPlayerData().Gear = 100;
        GameManager.Instance.PlayerGameObject.GetComponent<Player>().CurrentHealth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
