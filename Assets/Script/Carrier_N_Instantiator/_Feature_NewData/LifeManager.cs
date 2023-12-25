using System.Data.Common;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Proxies;
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
        플레이어 LifeManager에는 피격되었을때 잠시 무적판정이 들어가야한다.

    *********************************************************************************/
    public class LifeManager {

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

        public LifeManager(float maxHp, float defence) {
            MaxHp = new Stat(maxHp, E_STAT_USE_TYPE.Natural, OnMaxHpUpdated);
            CurrentHealth = maxHp;
            Defence = new Stat(defence, E_STAT_USE_TYPE.Natural, OnDefenceUpdated);
            IsDie = false;

            //DashSkill을 참고해서 어떻게 의존성을 맺었는지 확인
            OnHpUpdated     ??= (float val) => {};
            OnDamaged       ??= (float val) => {};
            OnHeal          ??= (float val) => {};
            OnEnterDie      ??= () => {};
            OnExitDie       ??= () => {};
        }

        public LifeManager(float maxHp) : this(maxHp, 0){}

#endregion

#region Getter
        public bool IsDie {get; protected set;}
        public bool IsHit {get; protected set;}

        public float GetHpRatio() { return CurrentHealth / MaxHp; }

#endregion

#region Setter 
        
#endregion

#region Event Adder
        protected UnityAction<float> OnHpUpdated = null;
        public LifeManager AddOnUpdateEvent(UnityAction<float> action) {
            this.OnHpUpdated += action;
            return this;
        }
        
        protected UnityAction<float> OnDamaged = null;
        public LifeManager AddOnDamageEvent(UnityAction<float> action) {
            this.OnDamaged += action;
            return this;
        }         
        protected UnityAction<float> OnHeal = null;
        public LifeManager AddOnHealEvent(UnityAction<float> action) {
            this.OnHeal += action;
            return this;
        } 
        protected UnityAction OnEnterDie = null;
        public LifeManager AddOnEnterDieEvent(UnityAction action) {
            this.OnEnterDie += action;
            return this;
        } 
        protected UnityAction OnExitDie = null;
        public LifeManager AddOnExitDieEvent(UnityAction action) {
            this.OnExitDie += action;
            return this;
        } 

        public void ClearEvents() {
            OnHpUpdated     = null;
            OnDamaged       = null;
            OnHeal          = null;
            OnEnterDie      = null;
            OnExitDie       = null;
            

            OnHpUpdated     ??= (float val) => {};
            OnDamaged       ??= (float val) => {};
            OnHeal          ??= (float val) => {};
            OnEnterDie      ??= () => {};
            OnExitDie       ??= () => {};
        }

#endregion
        // DasSkill과 연관시키기
        protected void OnMaxHpUpdated() { 
            throw new System.NotImplementedException();
        }

        protected void OnDefenceUpdated() { return; }

        public void GetHeal(float amount) {
            OnHeal.Invoke(amount);
            CurrentHealth += amount;
            OnHpUpdated.Invoke(CurrentHealth);
        }
        
        public void GetDamaged(float damage) {
            OnDamaged.Invoke(damage);
            CurrentHealth -= damage;
            Debug.Log($"데미지 : {damage}, 현재 HP : {CurrentHealth}");
            if (CurrentHealth <= 0) { this.Die(); }
            OnHpUpdated.Invoke(CurrentHealth);
        }

        public void Die() { 
            OnEnterDie.Invoke();
            IsDie = true; 
            OnExitDie.Invoke();
        }
    }
}