using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractEquipment : MonoBehaviour {
    public string equipmentName;
    public string description;
    public Sprite sprite;
    [SerializeField] public MasterData AddingData;
    public bool mIsInitialized = false;
    
    public abstract void InitEquipment(int _selectIndex);
}