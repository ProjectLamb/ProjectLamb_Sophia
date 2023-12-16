using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace  Feature_NewData
{
    public abstract class Entity : MonoBehaviour, IDamagable, IDieable, IDataAccessable {
        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;

        /*Visuality*/
        [SerializeField] public GameObject model;
        [SerializeField] public VisualModulator visualModulator;
        

        public abstract void GetDamaged(int _amount);
        public abstract void GetDamaged(int _amount, VFXObject _vfx);
        public abstract void Die();

        public abstract Stat GetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType);

        public abstract void SetNumeric(E_NUMERIC_STAT_MEMBERS numericMemberType, float value, E_STAT_USE_TYPE useType);

        public abstract void AddCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);

        public abstract void RemoveCalculator(E_NUMERIC_STAT_MEMBERS numericMemberType, StatCalculator calc);

        public abstract void ResetCalculators(E_NUMERIC_STAT_MEMBERS numericMemberType);

        public abstract UnityAction GetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType);

        public abstract void SetFunctional(E_FUNCTIONAL_ACTION_MEMBERS functionalActionType, UnityAction action);
    }
}
