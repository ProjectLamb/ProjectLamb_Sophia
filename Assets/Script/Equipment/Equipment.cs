using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*********************************************************************************
*
* 먹었을떄 적용되며, 해제될떄 적용 될것
*
*********************************************************************************/

public abstract class Equipment : MonoBehaviour{
    [Header("부품 이름")]
    [SerializeField]
    public string equipmentName;
    public string description;
    protected bool mIsApplyed;

    //public virtual void Equip(ref PlayerData pd){}
    //public virtual void Unequip(ref PlayerData pd){}
}