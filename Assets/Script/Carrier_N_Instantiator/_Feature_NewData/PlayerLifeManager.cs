using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public class PlayerLifeManager : LifeManager
    {
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

        public PlayerLifeManager(float maxHp, float defence) : base(maxHp, defence) {}

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

//      public bool IsDie {get; protected set;}

        public override void Die()
        {
            throw new System.NotImplementedException();
        }
        public override void GetDamaged(float damage)
        {
            OnDamaged.Invoke(ref damage);
            damage = (int)(damage * 100/(100+PlayerDataManager.GetEntityData().Defence));
            if(CurrentBarrierAmount >= 0.001f) {
                if(CurrentBarrierAmount - damage >= 0) {damage = 0; CurrentBarrierAmount -= damage;}
                else {
                    damage = damage - CurrentBarrierAmount; 
                    CurrentBarrierAmount = 0;
                }
            }
            CurrentHealth -= damage;
            if(CurrentHealth <= 0.001f) {Die();}
        }

        public override void GetDamaged(float damage, VFXObject vfx)
        {
            OnDamaged.Invoke(ref damage);
            damage = (int)(damage * 100/(100+Defence));
            if(CurrentBarrierAmount >= 0.001f) {
                if(CurrentBarrierAmount - damage >= 0) {damage = 0; CurrentBarrierAmount -= damage;}
                else {
                    damage = damage - CurrentBarrierAmount; 
                    CurrentBarrierAmount = 0;
                }
            }
            CurrentHealth -= damage;
            if(CurrentHealth <= 0.001f) {Die();}
            // VFX;
        }

        protected override void OnDefenceUpdated()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnMaxHpUpdated()
        {
            throw new System.NotImplementedException();
        }
    }
}