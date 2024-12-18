using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/Weapon", order = int.MaxValue)]
public class ScriptableObjWeaponData : ScriptableObject {
    public WEAPON_TYPE  WeaponType;
    public string       WeaponName;
    public Sprite       WeaponIcon;
    public Animator     WeaponAnimation;

    [TextArea]
    public string       WeaponDescription;
    public float        DamageRatio;
    public float        WeaponDelay;
    public float        Range;
    public int          Ammo;
    public UnityAction  WeaponUseState = () => {};
    public UnityAction  WeaponChangeState = () => {};
    public UnityAction  WeaponReLoadState = () => {};
}