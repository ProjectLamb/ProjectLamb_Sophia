using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : Purchase
{
    public GameObject moneyItem;
    public GameObject heartItem;
    public GameObject equipmentItem;
    float[] probs;
    float totalProbs = 100.0f;
    public int price;
    // Start is called before the first frame update
    void Start()
    {
        probs = new float[7] { 5.0f, 10.0f, 10.0f, 15.0f, 20.0f, 20.0f, 20.0f };
    }

    int Gacha()
    {
        int returnValue = 0;
        float randomValue = Random.value * totalProbs;

        float temp = 0.0f;

        for (int i = 0; i < 7; i++)
        {
            temp += probs[i];
            if (randomValue <= temp)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (purchase(price))
            {
                switch (Gacha())
                {
                    case 0:
                    Debug.Log("부품");
                        break;
                    case 1:
                    Debug.Log("체력");
                        break;
                    case 2:
                    Debug.Log("30원");
                        break;
                    case 3:
                    Debug.Log("20원");
                        break;
                    case 4:
                    Debug.Log("10원");
                        break;
                    case 5:
                    Debug.Log("5원");
                        break;
                    case 6:
                    Debug.Log("1원");
                        break;
                }
            }
        }
    }
}
