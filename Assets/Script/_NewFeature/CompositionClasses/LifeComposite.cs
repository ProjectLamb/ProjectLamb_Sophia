using UnityEngine;
using UnityEngine.Events;
using Sophia.DataSystem;

using Sophia.DataSystem.Referer;
using System.Collections.Generic;
using Sophia.UserInterface;
using System;


namespace Sophia
{
    public enum DamageHandleType { None, BarrierCoved, BarrierPiercing, Dodge }
    public enum HitType { None, Critical, TrueDamage }

    [System.Serializable]
    public struct DamageInfo
    {
        [SerializeField] public int damageAmount;
        [SerializeField] public float damageRatio;
        [SerializeField] public DamageHandleType damageHandleType;
        [SerializeField] public HitType hitType;

        public int GetAmount() => (int)(damageAmount * damageRatio);
        public override string ToString()
        {
            switch (damageHandleType)
            {
                case DamageHandleType.Dodge: { return $"<#666><i>Dodged<i></color>"; }
                case DamageHandleType.BarrierPiercing: { return $"<#FFF><b>{damageAmount * damageRatio}</b></color>"; }
                case DamageHandleType.BarrierCoved: { return $"<#FF0><i>Blocked!<i></color>"; }
                default: { break; }
            }
            switch (hitType)
            {
                case HitType.TrueDamage: { return $"<#F0F><b>!{damageAmount * damageRatio}!</b><color>"; }
                case HitType.Critical: { return $"<b><#F00>!{damageAmount * damageRatio}!</color></b>"; }
                default: { return $"{damageAmount * damageRatio}"; }
            }
        }
    }

    namespace Composite
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

        public class LifeComposite : IDataSettable, IDisposable
        {

#region Members 

            public Stat                 MaxHp           { get; protected set; }
            public Stat                 Defence         { get; protected set; }
            public Extras<DamageInfo>   DamagedExtras   { get; protected set; }
            public Extras<object>       DeadExtras      { get; protected set; }
            public Extras<int>          HealExtras      { get; protected set; }

            private float mCurrentHealth;
            public float CurrentHealth
            {
                get { return mCurrentHealth; }
                protected set
                {
                    int floatToIntHp = MaxHp;
                    if (value > (float)floatToIntHp)
                    {
                        mCurrentHealth = (float)floatToIntHp; 
                        OnHpUpdated?.Invoke(mCurrentHealth);
                        return;
                    }
                    if (value < 0) { mCurrentHealth = 0; return; }
                    mCurrentHealth = value;
                    OnHpUpdated?.Invoke(mCurrentHealth);
                }
            }

            private int mCurrentBarrier;
            public int CurrentBarrier
            {
                get { return mCurrentBarrier; }
                protected set
                {
                    if (value <= 0.01f)
                    {
                        IsBarrierCovered = false;
                        mCurrentBarrier = 0;
                        return;
                    }
                    else
                    {
                        IsBarrierCovered = true;
                        mCurrentBarrier = value;
                        return;
                    }
                }
            }

            public LifeComposite(float maxHp, float defence)
            {
                MaxHp           = new Stat(maxHp, E_NUMERIC_STAT_TYPE.MaxHp, E_STAT_USE_TYPE.Natural, OnMaxHpUpdated);
                Defence         = new Stat(defence, E_NUMERIC_STAT_TYPE.Defence, E_STAT_USE_TYPE.Natural, OnDefenceUpdated);
                DamagedExtras   = new Extras<DamageInfo>(E_FUNCTIONAL_EXTRAS_TYPE.Damaged, OnDamageExtrasUpdated);
                DeadExtras      = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Dead, OnDeadExtrasUpdated);
                HealExtras      = new Extras<int>(E_FUNCTIONAL_EXTRAS_TYPE.HealthTriggered, OnHealTriggeredExtrasUpdated);

                CurrentHealth = maxHp;
                IsDie = false;
                CurrentBarrier = 0;

                //DashSkill을 참고해서 어떻게 의존성을 맺었는지 확인
                OnHpUpdated ??= (float val) => { };
                OnBarrierUpdated ??= (float val) => { };
                OnDamaged ??= (DamageInfo val) => { };
                OnHeal ??= (int val) => { };
                OnBarrier ??= (float val) => { };
                OnEnterDie ??= () => { };
                OnExitDie ??= () => { };
                OnBreakBarrier ??= () => { };
            }

            public LifeComposite(float maxHp) : this(maxHp, 0) { }

#endregion

#region Getter

            public bool IsDie { get; protected set; }

            public bool IsHit { get; protected set; }

            public bool IsBarrierCovered { get; protected set; }

            public float GetHpRatio() { return CurrentHealth / MaxHp; }

#endregion

#region Setter 

            public void SetDependUI(IHealthBarUI<LifeComposite> healthBar) => healthBar.SetReferenceComposite(this);

#endregion

#region Event
            public event UnityAction<float> OnHpUpdated = null;
            public event UnityAction<float> OnBarrierUpdated = null;
            public event UnityAction<DamageInfo> OnDamaged = null;
            public event UnityAction<int> OnHeal = null;
            public event UnityAction OnEnterDie = null;
            public event UnityAction OnExitDie = null;
            public event UnityAction<float> OnBarrier = null;
            public event UnityAction OnBreakBarrier = null;

            protected void OnMaxHpUpdated()         => Debug.Log("최대체력 변경됨!");
            protected void OnDefenceUpdated()       => Debug.Log("방어력 변경됨!");
            protected void OnBarrierRatioUpdated()  => Debug.Log("배리어 변경됨!");
            protected void OnHitExtrasUpdated()     => Debug.Log("닿음 추가 동작 변경됨!");
            protected void OnDamageExtrasUpdated()  => Debug.Log("피격 추가 동작 변경됨!");
            protected void OnDeadExtrasUpdated()    => Debug.Log("죽음 추가 동작 변경됨!");
            protected void OnHealTriggeredExtrasUpdated()    => Debug.Log("회복 추가 동작 변경됨!");
            
            public void ClearEvents()
            {
                OnHpUpdated = null;
                OnBarrierUpdated = null;
                OnDamaged = null;
                OnHeal = null;
                OnEnterDie = null;
                OnExitDie = null;
                OnBarrier = null;
                OnBreakBarrier = null;

                OnHpUpdated ??= (float val) => { };
                OnBarrierUpdated ??= (float val) => { };
                OnDamaged ??= (DamageInfo val) => { };
                OnHeal ??= (int val) => { };
                OnEnterDie ??= () => { };
                OnExitDie ??= () => { };
                OnBarrier ??= (float val) => { };
                OnBreakBarrier ??= () => { };
            }

#endregion

#region Data Referer 

            public void SetStatDataToReferer(EntityStatReferer statReferer)
            {
                statReferer.SetRefStat(MaxHp);
                statReferer.SetRefStat(Defence);
            }

            public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
            {
                extrasReferer.SetRefExtras<DamageInfo>(DamagedExtras);
                extrasReferer.SetRefExtras<object>(DeadExtras);
                extrasReferer.SetRefExtras<int>(HealExtras);
            }
#endregion
            public void Healed(int amount)
            {
                OnHeal?.Invoke(amount);
                HealExtras.PerformStartFunctionals(ref amount);
                CurrentHealth += amount;
            }

            public void BarrierCoverd(int amount)
            {
                CurrentBarrier += amount;
                OnBarrier?.Invoke(amount);
            }

            public void SetBarrier(int amount)
            {
                CurrentBarrier = amount;
                OnBarrier?.Invoke(amount);
            }

            public bool Damaged(DamageInfo damageInfo)
            {
                DamagedExtras.PerformStartFunctionals(ref damageInfo);
                switch (damageInfo.damageHandleType)
                {
                    case DamageHandleType.Dodge:
                        {
                            return false;
                        }
                    case DamageHandleType.BarrierPiercing:
                        {
                            CurrentHealth -= damageInfo.GetAmount();
                            break;
                        }
                    default:
                        {
                            int InputDamage = damageInfo.GetAmount();
                            if(CurrentBarrier > 0) {
                                if (CurrentBarrier - InputDamage >= 0)
                                {
                                    CurrentBarrier -= InputDamage;
                                    InputDamage = 0;
                                    damageInfo.damageHandleType = DamageHandleType.BarrierCoved;
                                    OnBarrierUpdated?.Invoke(CurrentBarrier);
                                }
                                else
                                {
                                    InputDamage -= (int)CurrentBarrier;
                                    BreakBarrier();
                                    damageInfo.damageHandleType = DamageHandleType.None;
                                }
                            } 
                            CurrentHealth -= InputDamage;
                            break;
                        }
                }

                OnDamaged?.Invoke(damageInfo);

                DamagedExtras.PerformExitFunctionals(ref damageInfo);
                if (CurrentHealth <= 0) { this.Died(); }
                return true;
            }

            public void BreakBarrier()
            {
                CurrentBarrier = 0;
                OnBreakBarrier?.Invoke();
            }

            public void Died()
            {
                object nullObject = null;
                DeadExtras.PerformStartFunctionals(ref nullObject);
                OnEnterDie?.Invoke();
                IsDie = true;
                DeadExtras.PerformExitFunctionals(ref nullObject);
                OnExitDie?.Invoke();
            }

            public void Dispose()
            {
                OnHpUpdated = null;
                OnBarrierUpdated = null;
                OnDamaged = null;
                OnHeal = null;
                OnEnterDie = null;
                OnExitDie = null;
                OnBarrier = null;
                OnBreakBarrier = null;
            }
        }
    }
}