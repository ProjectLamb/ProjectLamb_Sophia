using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameObject[] tileArray;
    public int i;
    public int j;

    void Awake()
    {
        tileArray = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            tileArray[i] = transform.GetChild(i).gameObject;
        }
        int randomValue = Random.Range(0, 4);
        tileArray[randomValue].SetActive(true);
    }

    void Start()
    {

    }
}