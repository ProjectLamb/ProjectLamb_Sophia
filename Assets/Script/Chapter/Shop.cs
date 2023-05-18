using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject equipment;
    public GameObject skill;
    public GameObject heart;
    public GameObject rerollMachine;
    public GameObject vendingMachine;
    private int equipmentCount = 2;
    private int heartCount = 0;
    private int equipmentPrice = 20;
    private int heartPrice = 10;
    public GameObject[] rerollable;

    void Awake()
    {
        rerollable = new GameObject[2];
        rerollable[0] = equipment;
        rerollable[1] = skill;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
