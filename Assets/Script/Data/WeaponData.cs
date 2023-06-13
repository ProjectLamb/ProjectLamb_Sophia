using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_WeaponType
{
    melee, ranger, mage
}

[System.Serializable]
public struct WeaponData
{
    public E_WeaponType WeaponType;
    public string WeaponName;
    public string WeaponDescription;
    public float DamageRatio;
    public float WeaponDelay;
    public float Range;
    public float Ammo;
    public UnityAction WeaponUseState;
    public UnityAction WeaponChangeState;
    public UnityAction WeaponReLoadState;
    public WeaponData(ScriptableObjWeaponData _scriptable)
    {
        WeaponType = _scriptable.WeaponType;
        WeaponName = _scriptable.WeaponName;
        WeaponDescription = _scriptable.WeaponDescription;
        DamageRatio = _scriptable.DamageRatio;
        WeaponDelay = _scriptable.WeaponDelay;
        Range = _scriptable.Range;
        Ammo = _scriptable.Ammo;
        WeaponUseState = _scriptable.WeaponUseState;
        WeaponChangeState = _scriptable.WeaponChangeState;
        WeaponReLoadState = _scriptable.WeaponReLoadState;
    }
    public static WeaponData operator +(WeaponData x, WeaponData y)
    {
        WeaponData res = new WeaponData();
        res = x;
        res.DamageRatio += y.DamageRatio;
        res.WeaponDelay += y.WeaponDelay;
        res.Range += y.Range;
        res.WeaponUseState += y.WeaponUseState;
        res.WeaponChangeState += y.WeaponChangeState;
        res.WeaponReLoadState += y.WeaponReLoadState;
        return res;
    }
    public static WeaponData operator -(WeaponData x, WeaponData y)
    {
        WeaponData res = new WeaponData();
        res = x;
        res.DamageRatio -= y.DamageRatio;
        res.WeaponDelay -= y.WeaponDelay;
        res.Range -= y.Range;
        res.WeaponUseState -= y.WeaponUseState;
        res.WeaponChangeState -= y.WeaponChangeState;
        res.WeaponReLoadState -= y.WeaponReLoadState;
        return res;
    }
    public readonly override string ToString() {
        return $"DamageRatio : {DamageRatio}, WeaponDelay : {WeaponDelay}, Range : {Range}, Ammo : {Ammo}\n";
    }
}