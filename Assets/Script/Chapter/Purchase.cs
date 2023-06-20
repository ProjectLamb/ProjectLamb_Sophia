using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public bool purchase(int price)
    {
        int current_gear = GameManager.Instance.playerGameObject.GetComponent<Player>().playerData.Gear;

        if (current_gear >= price)  //구매 가능
        {
            GameManager.Instance.playerGameObject.GetComponent<Player>().playerData.Gear -= price;
            return true;
        }
        else
            return false;
    }
}
