using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public class EnemyLifeManager : LifeManager {

#region Members 

//      public Stat MaxHp {get; protected set;}
//      public Stat Defence {get; protected set;}
//
//      private float mCurrentHealth;
//
//      public float CurrentHealth {
//          get {return mCurrentHealth;}
//          protected set {
//              if(value > MaxHp) {
//                  mCurrentHealth = MaxHp; return;
//              }
//              if(value < 0) {mCurrentHealth = 0; return;}
//              mCurrentHealth = value;
//          }
//      }

        public EnemyLifeManager(float maxHp, float defence) : base(maxHp, defence) {}
        public EnemyLifeManager(float maxHp) : base(maxHp) {}
        
#endregion

#region Event Adder

//      private event UnityActionRef<float> OnDamaged;
//      public LifeManager AddOnDamageEvent(UnityActionRef<float> action) {
//          this.OnDamaged += action;
//          return this;
//      } 
//      private event UnityAction OnEnterDie;
//      public LifeManager AddOnEnterDieEvent(UnityAction action) {
//          this.OnEnterDie += action;
//          return this;
//      } 
//      private event UnityAction OnExitDie;
//      public LifeManager AddOnExitDieEvent(UnityAction action) {
//          this.OnExitDie += action;
//          return this;
//      } 

#endregion

        protected sealed override void OnMaxHpUpdated() { 
            throw new NotImplementedException();
        }

        protected sealed override void OnDefenceUpdated() { return; }

        public override void GetHeal(float amount) {
            CurrentHealth += amount;
        }
        
        public override void GetDamaged(float damage) {
            OnDamaged.Invoke(damage);
            CurrentHealth -= damage;
            if (CurrentHealth <= 0) { this.Die(); }
        }

        public override void Die() { 
            OnEnterDie.Invoke();
            IsDie = true; 
            OnExitDie.Invoke();
        }
    }
}