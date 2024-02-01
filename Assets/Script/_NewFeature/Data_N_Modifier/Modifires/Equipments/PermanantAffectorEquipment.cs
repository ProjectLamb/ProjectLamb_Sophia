using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using Sophia;
using Sophia.DataSystem.Modifiers.ConcreteAffectors;
using Sophia.DataSystem.Modifiers.Affector;
using UnityEngine.UIElements;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem;
using Sophia.Instantiates;

public class PermanantAffectorEquipment : Carrier { //, IPlayerDataApplicant{

    ExtrasModifier<Sophia.Entitys.Entity> extrasModifier;
    Sophia.Entitys.Entity OwnerRef;

    private void Awake() {
        
        /******************************************************

        Extras<Sophia.Entitys.Entity> TargetAffectedExtrasRef
            = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
        
        ******************************************************/
        
        /******************************************************
        
        ExtrasModifier<Sophia.Entitys.Entity> extrasModifier 
            = new ExtrasModifier<Sophia.Entitys.Entity>( UnityActionRef<Sophia.Entitys.Entity> ));

            UnityActionRef<Sophia.Entitys.Entity> 내부는 반드시 Affector 객체가 생겨야 한다.
        
        ******************************************************/

        /******************************************************
        
        TargetAffectedExtrasRef.AddModifier(this.extrasModifier);
        
        ******************************************************/
        UnityActionRef<Sophia.Entitys.Entity> affectAction = null;
        switch(_affectType) {
            case E_AFFECT_TYPE.Poisoned : {affectAction = OnPoisionAffectedToTarget; break;}
            case E_AFFECT_TYPE.Stern    : {affectAction = OnSternAffectedToTarget; break;}
            case E_AFFECT_TYPE.Airborn    : {affectAction = OnAirbornAffectedToTarget; break;}
            default : {throw new System.Exception("현재 알맞는 어펙터가 없음");}
        }

        extrasModifier = new ExtrasModifier<Sophia.Entitys.Entity>(affectAction, E_EXTRAS_PERFORM_TYPE.Tick, E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
    }
    [SerializeField] public E_AFFECT_TYPE _affectType;
    [SerializeField] public Material _material;
    [SerializeField] public Sophia.Instantiates.VisualFXObject _visualFx;
    [SerializeField] public float _baseDurateTime;
    [SerializeField] public float _intervalTime;
    [SerializeField] public float _TickDamageRatio;
    [SerializeField] private float _TickDamage;
    public void OnPoisionAffectedToTarget(ref Sophia.Entitys.Entity target) {
        Affector PoisionAffector = new PoisonedAffect(this.OwnerRef, target, this._baseDurateTime)
                                        .SetIntervalTime(this._intervalTime)
                                        .SetTickDamage(this._TickDamage)
                                        .SetTickDamageRatio(this._TickDamageRatio)
                                        .SetMaterial(this._material)
                                        .SetVisualFXObject(this._visualFx);
        PoisionAffector.ConveyToTarget();
    }

    public void OnSternAffectedToTarget(ref Sophia.Entitys.Entity target) {
        Affector SternAffector = new SternAffect(this.OwnerRef, target, this._baseDurateTime)
                                        .SetMaterial(this._material)
                                        .SetVisualFXObject(this._visualFx);
        SternAffector.ConveyToTarget();
    }
    public void OnAirbornAffectedToTarget(ref Sophia.Entitys.Entity target) {
        Affector SternAffector = new AirborneAffect(this.OwnerRef, target, this._baseDurateTime);
        SternAffector.ConveyToTarget();
    }

    protected override void OnTriggerLogic(Collider entity)
    {
        if(entity.TryGetComponent<Sophia.Entitys.Player>(out Sophia.Entitys.Player player)){
            Debug.Log("Triggerd");
            OwnerRef = player;
            Extras<Sophia.Entitys.Entity> extrasRef = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
            extrasRef.AddModifier(this.extrasModifier);
            Destroy(this.gameObject);
        }
        else {
            Debug.Log("entity는 Player가 아니거나 못찾음" );
        }
    }
}
