using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using Sophia_Carriers;
using Sophia.DataSystem.Modifiers.ConcreteAffectors;
using Sophia.DataSystem.Modifiers.Affector;
using UnityEngine.UIElements;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem;

public class TEST_Equipment_024 : Equipment { //, IPlayerDataApplicant{

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

        extrasModifier = new ExtrasModifier<Sophia.Entitys.Entity>(OnAffectedToTarget, E_EXTRAS_PERFORM_TYPE.Tick, E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Sophia.Entitys.Player>(out Sophia.Entitys.Player player)){
            OwnerRef = player;
            Extras<Sophia.Entitys.Entity> extrasRef = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected);
            extrasRef.AddModifier(this.extrasModifier);
        }
        base.OnTriggerEnter(other);
    }

    [SerializeField] public Material _material;
    [SerializeField] public Sophia.Instantiates.VisualFXObject _visualFx;
    [SerializeField] public float _baseDurateTime;
    [SerializeField] public float _intervalTime;
    [SerializeField] public float _TickDamageRatio;
    [SerializeField] private float _TickDamage;
    public void OnAffectedToTarget(ref Sophia.Entitys.Entity target) {
        Affector PoisionAffector = new PoisonedAffect(this.OwnerRef, target, this._baseDurateTime)
                                        .SetIntervalTime(this._intervalTime)
                                        .SetTickDamage(this._TickDamage)
                                        .SetTickDamageRatio(this._TickDamageRatio)
                                        .SetMaterial(this._material)
                                        .SetVisualFXObject(this._visualFx);
        PoisionAffector.ConveyToTarget();
    }
}
