using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponData {
    public E_WeaponType WeaponType;
    public string WeaponName;
    public string WeaponDescription;
    
    public float DamageRatio {get; set;}
    public float WeaponDelay {get; set;}
    public float Range {get; set;}
        
            private int mAmmo;

    public int Ammo {
        get{
            if(this.WeaponType != E_WeaponType.ranger){
                throw new System.Exception("원거리 무기가 아니므로 접근 불가능");
            }
            return mAmmo;
        }
        set {
            if(this.WeaponType != E_WeaponType.ranger){
                throw new System.Exception("원거리 무기가 아니므로 접근 불가능");
            }
            mAmmo = value;
        }
    }

    public List<Projectile> Projectile;

    public UnityAction UseState;
    public UnityAction ChangeState;
    public UnityAction ReLoadState; 

    public WeaponData(ScriptableObjWeaponData _weaponScriptable) {
        this.WeaponType = _weaponScriptable.weaponType;
        this.WeaponName = _weaponScriptable.weaponName;
        this.WeaponDescription = _weaponScriptable.weaponDescription;
        this.DamageRatio = _weaponScriptable.damageRatio;
        this.WeaponDelay = _weaponScriptable.weaponDelay;
        this.Range = _weaponScriptable.range;
        this.Projectile = new List<Projectile>(_weaponScriptable.projectiles);
        UseState = () => {};
        ChangeState = () => {};

        if(WeaponType == E_WeaponType.ranger){
            this.Ammo = _weaponScriptable.ammo;
            ReLoadState = () => {};
        }
    }
}