using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public abstract class LifeManager {

#region Members 
        
        public Stat MaxHp {get; protected set;}
        public Stat Defence {get; protected set;}

        private float mCurrentHealth;
 
        public float CurrentHealth {
            get {return mCurrentHealth;}
            protected set {
                if(value > MaxHp) {
                    mCurrentHealth = MaxHp; return;
                }
                if(value < 0) {mCurrentHealth = 0; return;}
                mCurrentHealth = value;
            }
        }
        public float CurrentBarrierAmount {get; protected set;}

        public LifeManager(float maxHp, float defence) {
            MaxHp = new Stat(maxHp, E_STAT_USE_TYPE.Natural, OnMaxHpUpdated);
            Defence = new Stat(defence, E_STAT_USE_TYPE.Natural, OnDefenceUpdated);
            IsDie = false;
        }

        public LifeManager(float maxHp) : this(maxHp, 0){}

#endregion

#region Event Adder
        protected UnityActionRef<float> OnDamaged;
        public LifeManager AddOnDamageEvent(UnityActionRef<float> action) {
            this.OnDamaged += action;
            return this;
        } 
        protected UnityAction OnEnterDie;
        public LifeManager AddOnEnterDieEvent(UnityAction action) {
            this.OnEnterDie += action;
            return this;
        } 
        protected UnityAction OnExitDie;
        public LifeManager AddOnExitDieEvent(UnityAction action) {
            this.OnExitDie += action;
            return this;
        } 

#endregion

        protected abstract void OnMaxHpUpdated();
        protected abstract void OnDefenceUpdated();

        public virtual void GetHeal(float amount) {
            CurrentHealth += amount;
        }
        
        public abstract void GetDamaged(float damage);
        public abstract void GetDamaged(float damage, VFXObject vfx);
        
        public bool IsDie {get; protected set;}

        public abstract void Die();
    }
}