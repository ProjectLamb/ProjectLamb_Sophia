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

        public LifeManager(float maxHp, float defence) {
            MaxHp = new Stat(maxHp, E_STAT_USE_TYPE.Natural, OnMaxHpUpdated);
            Defence = new Stat(defence, E_STAT_USE_TYPE.Natural, OnDefenceUpdated);
            IsDie = false;

            //DashSkill을 참고해서 어떻게 의존성을 맺었는지 확인
        }

        public LifeManager(float maxHp) : this(maxHp, 0){}

#endregion

#region Getter
        public bool IsDie {get; protected set;}
        public bool IsHit {get; protected set;}
#endregion

#region Setter 
        
#endregion

#region Event Adder
        protected UnityAction<float> OnDamaged;
        public LifeManager AddOnDamageEvent(UnityAction<float> action) {
            this.OnDamaged += action;
            return this;
        }         
        protected UnityAction<float> OnHeal;
        public LifeManager AddOnHealEvent(UnityAction<float> action) {
            this.OnHeal += action;
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
        // DasSkill과 연관시키기
        protected abstract void OnMaxHpUpdated();
        protected abstract void OnDefenceUpdated();

        public virtual void GetHeal(float amount) { CurrentHealth += amount; }
        public abstract void GetDamaged(float damage);

        public abstract void Die();
    }
}