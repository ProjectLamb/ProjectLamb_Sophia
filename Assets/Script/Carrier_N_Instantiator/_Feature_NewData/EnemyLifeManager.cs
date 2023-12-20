using System.Runtime.InteropServices;
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

        protected override void OnMaxHpUpdated() {

        }
        protected override void OnDefenceUpdated() {

        }

        public override void GetHeal(float amount) {
            CurrentHealth += amount;
        }
        
        public override void GetDamaged(float damage) {

        }
        public override void GetDamaged(float damage, VFXObject vfx) {

        }
        
//      public bool IsDie {get; protected set;}

        public override void Die() {

        }
    }
}