using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class Equipment : MonoBehaviour{
    public string EquipmentName;
    [SerializeField]
    protected PlayerData playerData = new PlayerData();
    
    [SerializeField]
    protected WeaponData weaponData = new WeaponData();
    [SerializeField]
    protected SkillData skillData = new SkillData();

    public abstract void Adaptation(PlayerData _input);
}