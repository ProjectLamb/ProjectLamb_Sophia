using System.Data.Common;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    /*********************************************************************************
    *
    * 의존관계
         DamageUI Instantiator ❌
            DamageUI와 연동이 될것 ❌
        
        없고, 오직 Hp, Defence에 대해 
        체력 계산만 할뿐,

        그리고 IsDie인가 아닌가만 체크를 한다.

    * 특수화
        플레이어 LifeComposite에는 피격되었을때 잠시 무적판정이 들어가야한다.

    *********************************************************************************/
    public class LifeComposite {

#region Members 
        
        public Stat MaxHp {get; protected set;}
        public Stat Defence {get; protected set;}

        private float mCurrentHealth;
 
        public float CurrentHealth {
            get {return mCurrentHealth;}
            protected set {
                int floatToIntHp = MaxHp;
                if(value > (float)floatToIntHp) {
                    mCurrentHealth = (float)floatToIntHp; return;
                }
                if(value < 0) {mCurrentHealth = 0; return;}
                mCurrentHealth = value;
            }
        }

        private float mCurrentBarrier;
        public float CurrentBarrier {
            get {return mCurrentBarrier;}
            protected set {
                if(value <= 0.001f) {
                    IsBarrierCovered = false;
                    mCurrentBarrier = 0; 
                    return;
                }
                else {
                    IsBarrierCovered = true;
                    mCurrentBarrier = value;
                    return;
                }
            }
        }

        public LifeComposite(float maxHp, float defence) {
            MaxHp = new Stat( maxHp,
                E_NUMERIC_STAT_TYPE.MaxHp, 
                E_STAT_USE_TYPE.Natural, OnMaxHpUpdated
            );
            Defence = new Stat( defence,
                E_NUMERIC_STAT_TYPE.Defence, 
                E_STAT_USE_TYPE.Natural, OnDefenceUpdated
            );
            
            CurrentHealth = maxHp;
            IsDie = false;
            CurrentBarrier = 0;

            //DashSkill을 참고해서 어떻게 의존성을 맺었는지 확인
            OnHpUpdated      ??= (float val) => {};
            OnBarrierUpdated ??= (float val) => {};
            OnHit            ??= (ref float val) => {};
            OnDamaged        ??= (float val) => {};
            OnHeal           ??= (float val) => {};
            OnBarrier        ??= (float val) => {};
            OnEnterDie       ??= () => {};
            OnExitDie        ??= () => {};
            OnBreakBarrier   ??= () => {};
        }

        public LifeComposite(float maxHp) : this(maxHp, 0){}

#endregion

#region Getter

        public bool IsDie {get; protected set;}

        public bool IsHit {get; protected set;}

        public bool IsBarrierCovered {get; protected set;}

        public float GetHpRatio() { return CurrentHealth / MaxHp; }

#endregion

#region Setter 
        
#endregion

#region Event Adder

        protected UnityAction<float> OnHpUpdated = null;
        public LifeComposite AddOnUpdateEvent(UnityAction<float> action) {
            this.OnHpUpdated += action;
            return this;
        }

        protected UnityAction<float> OnBarrierUpdated = null;
        public LifeComposite AddOnBarrierUpdateEvent(UnityAction<float> action) {
            this.OnBarrierUpdated += action;
            return this;
        }
        
        protected UnityAction<float> OnDamaged = null;
        public LifeComposite AddOnDamageEvent(UnityAction<float> action) {
            this.OnDamaged += action;
            return this;
        }         
        
        protected UnityAction<float> OnHeal = null;
        public LifeComposite AddOnHealEvent(UnityAction<float> action) {
            this.OnHeal += action;
            return this;
        } 
        
        protected UnityAction OnEnterDie = null;
        public LifeComposite AddOnEnterDieEvent(UnityAction action) {
            this.OnEnterDie += action;
            return this;
        } 

        protected UnityAction OnExitDie = null;
        public LifeComposite AddOnExitDieEvent(UnityAction action) {
            this.OnExitDie += action;
            return this;
        } 

        protected UnityAction<float> OnBarrier = null;
        public LifeComposite AddOnBarrierEvent(UnityAction<float> action) {
            this.OnBarrier += action;
            return this;
        } 

        protected UnityAction OnBreakBarrier = null;
        public LifeComposite AddOnBreakBarrierEvent(UnityAction action) {
            this.OnBreakBarrier += action;
            return this;
        } 

        protected UnityActionRef<float> OnHit = null;
        public LifeComposite AddOnHitEvent(UnityActionRef<float> actionRef) {
            this.OnHit += actionRef;
            return this;
        }

        public void ClearEvents() {
            OnHpUpdated     = null;
            OnBarrierUpdated = null;
            OnDamaged       = null;
            OnHeal          = null;
            OnEnterDie      = null;
            OnExitDie       = null;
            OnBarrier       = null;
            OnBreakBarrier  = null;
            OnHit           = null;

            OnHpUpdated     ??= (float val) => {};
            OnBarrierUpdated ??= (float val) => {};
            OnHit           ??= (ref float val) => {};
            OnDamaged       ??= (float val) => {};
            OnHeal          ??= (float val) => {};
            OnEnterDie      ??= () => {};
            OnExitDie       ??= () => {};
            OnBarrier       ??= (float val) => {};
            OnBreakBarrier  ??= () => {};
        }

#endregion

        // DasSkill과 연관시키기
        protected void OnMaxHpUpdated() { 
            throw new System.NotImplementedException();
        }

        protected void OnDefenceUpdated() { 
            throw new System.NotImplementedException();
        }
        
        protected void OnBarrierRatioUpdated() { 
            throw new System.NotImplementedException();
        }

        public void Healed(float amount) {
            OnHeal.Invoke(amount);
            CurrentHealth += amount;
            OnHpUpdated.Invoke(CurrentHealth);
        }

        public void BarrierCoverd(float amount) {
            OnBarrier.Invoke(amount);
            CurrentBarrier += amount;
            OnBarrierUpdated.Invoke(CurrentBarrier);
        }

        public void SetBarrier(float amount) {
            CurrentBarrier = amount;
        }
        
        public void Damaged(float damage) {
            OnHit.Invoke(ref damage);
            DamageCalculatePipeline(ref damage);
            if(damage < 0.001f) {return;}

            OnDamaged.Invoke(damage);
            CurrentHealth -= damage;
            OnHpUpdated.Invoke(CurrentHealth);

            if (CurrentHealth <= 0) { this.Died(); }
        }

        private void DamageCalculatePipeline(ref float _amount){
            _amount = _amount * 100 / (100+Defence);
            if(IsBarrierCovered && CurrentBarrier > 0){
                if(CurrentBarrier - _amount >= 0){
                    _amount = 0; CurrentBarrier -= _amount;
                }
                else {
                    _amount = _amount - CurrentBarrier; 
                    BreakBarrier();
                }
            }
        }

        public void BreakBarrier(){
            CurrentBarrier = 0;
            OnBreakBarrier.Invoke();
        }

        public void Died() { 
            OnEnterDie.Invoke();
            IsDie = true;
            OnExitDie.Invoke();
        }
    }
}