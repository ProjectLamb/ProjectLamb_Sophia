using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_WeaponType {
    melee, ranger, mage
}

[System.Serializable]
public class WeaponData {
    [field : SerializeField] public E_WeaponType WeaponType;
    [field : SerializeField] public string WeaponName;
    [field : SerializeField] public string WeaponDescription;
    
    [field : SerializeField] public float DamageRatio {get; set;}
    [field : SerializeField] public float WeaponDelay {get; set;}
    [field : SerializeField] public float Range {get; set;}
    
    public UnityAction WeaponUseState = () => {};
    public UnityAction WeaponChangeState = () => {};
    public UnityAction WeaponReLoadState = () => {}; 
    public WeaponData() {
        DamageRatio = 0f;
        WeaponDelay = 0f;
        Range       = 0f;
    }
    public WeaponData Clone() {
        WeaponData res = new WeaponData();
        res.WeaponType  = this.WeaponType;
        res.WeaponName  = this.WeaponName;
        res.WeaponDescription   = this.WeaponDescription;
        res.DamageRatio = this.DamageRatio;
        res.WeaponDelay = this.WeaponDelay;
        res.Range = this.Range;
        
        res.WeaponUseState = this.WeaponUseState;
        res.WeaponChangeState = this.WeaponChangeState;
        res.WeaponReLoadState = this.WeaponReLoadState;
        return res;
    }
    public static WeaponData operator +(WeaponData x, WeaponData y){
        WeaponData res = new WeaponData();
        res.DamageRatio = x.DamageRatio + y.DamageRatio;
        res.WeaponDelay = x.WeaponDelay + y.WeaponDelay;
        res.Range   = x.Range + y.Range;

        res.WeaponUseState = x.WeaponUseState + y.WeaponUseState;
        res.WeaponChangeState = x.WeaponChangeState + y.WeaponChangeState;
        res.WeaponReLoadState = x.WeaponReLoadState + y.WeaponReLoadState;
        return res;
    }
    public static WeaponData operator -(WeaponData x, MasterData y){
        WeaponData res = new WeaponData();
        res.DamageRatio = x.DamageRatio - y.DamageRatio;
        res.WeaponDelay = x.WeaponDelay - y.WeaponDelay;
        res.Range   = x.Range - y.Range;

        res.WeaponUseState = x.WeaponUseState - y.WeaponUseState;
        res.WeaponChangeState = x.WeaponChangeState - y.WeaponChangeState;
        res.WeaponReLoadState = x.WeaponReLoadState - y.WeaponReLoadState;
        return res;
    }
}