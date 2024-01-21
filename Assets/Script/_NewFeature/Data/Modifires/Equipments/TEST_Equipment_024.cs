using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;
using Sophia_Carriers;
using Sophia.DataSystem.ConcreteAffectors;
using Sophia.DataSystem.Affector;
using UnityEngine.UIElements;
using Sophia.DataSystem.Modifires;
using Sophia.DataSystem;

public class TEST_Equipment_024 : Equipment { //, IPlayerDataApplicant{
    [SerializeField] public Material _material;
    [SerializeField] public Sophia.Instantiates.VisualFXObject _visualFx;
    [SerializeField] public float _baseDurateTime;
    [SerializeField] public float _intervalTime;
    [SerializeField] public float _TickDamageRatio;
    [SerializeField] private float _TickDamage;
    ExtrasCalculator<Sophia.Entitys.Entity> extrasCalculator;

    Sophia.Entitys.Entity OwnerRef;
    private void Awake() {
        extrasCalculator = new ExtrasCalculator<Sophia.Entitys.Entity>(OnAffected, E_EXTRAS_PERFORM_TYPE.Tick, E_FUNCTIONAL_EXTRAS_TYPE.Affected);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Sophia.Entitys.Player>(out Sophia.Entitys.Player player)){
            OwnerRef = player;
            Extras<Sophia.Entitys.Entity> extrasRef = OwnerRef.GetExtras<Sophia.Entitys.Entity>(E_FUNCTIONAL_EXTRAS_TYPE.Affected);
            extrasRef.AddCalculator(this.extrasCalculator);
            extrasRef.RecalculateExtras();
        }
        base.OnTriggerEnter(other);
    }

    public void OnAffected(ref Sophia.Entitys.Entity target) {
        Affector PoisionAffector = new PoisonedAffect(this.OwnerRef, target)
                                            .SetIntervalTime(this._intervalTime)
                                            .SetTickDamage(this._TickDamage)
                                            .SetTickDamageRatio(this._TickDamageRatio)
                                            .SetDurateTime(this._baseDurateTime)
                                            .SetMaterial(this._material)
                                            .SetVisualFXObject(this._visualFx);
    }
}
