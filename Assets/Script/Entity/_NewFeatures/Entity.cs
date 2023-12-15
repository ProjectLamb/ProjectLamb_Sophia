using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace  Feature_NewData
{
    public abstract class Entity : MonoBehaviour, IDamagable, IDieable, INumericAccessable<EntityNumerics> , IFunctionalAccessable<EntityFunctionals> {
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;

        /*Visuality*/
        [SerializeField] public GameObject model;
        [SerializeField] public VisualModulator visualModulator;        

        public abstract void GetDamaged(int _amount);
        public abstract void GetDamaged(int _amount, VFXObject _vfx);
        public abstract void Die();

        public abstract EntityNumerics GetNumeric();

        public abstract void SetNumeric(EntityNumerics genericT);

        public abstract EntityFunctionals GetFunctional();

        public abstract void SetFunctional(EntityFunctionals genericT);
    }
}
