using UnityEngine;

namespace Sophia.DataSystem
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Sophia.Composite.Timer;
    using Sophia.DataSystem.Functional;
    using Sophia.Entitys;

    using UnityEngine.Events;

    namespace Modifiers.NewAffector
    {
        public class Affector : IUserInterfaceAccessible
        {
            #region Members

            public readonly E_AFFECT_TYPE AffectType;
            public readonly string Name;
            public readonly string Description;
            public readonly Sprite Icon;
            public readonly Dictionary<E_NUMERIC_STAT_TYPE, StatModifier> StatModifiers = new();
            public float BaseDurateTime { get; private set; }
            protected TimerComposite Timer { get; private set; }


            public Affector(SerialAffectorData affectData)
            {
                // Name = affectData. _equipmentName;
                // Description = affectData._description;
                // Icon = affectData._icon;
                foreach (E_NUMERIC_STAT_TYPE statType in Enum.GetValues(typeof(E_NUMERIC_STAT_TYPE)))
                {
                    SerialStatModifireDatas statValue = affectData._calculateAffectData.GetModifireDatas(statType);
                    if (statValue.calType != E_STAT_CALC_TYPE.None)
                    {
                        StatModifiers.Add(statType, new StatModifier(statValue.amount, statValue.calType, statType));
                    }
                }

                Timer = new TimerComposite(BaseDurateTime);
            }
            #endregion

            #region Setter
            public Affector SetAccelarationByTenacity(float tenecity)
            {
                Timer.SetAcceleratrion(tenecity);
                return this;
            }

            #endregion

            #region Event
            public event UnityAction OnRevert;

            #endregion

            public void Invoke(IDataAccessible dataAccessibleOwner)
            {
                foreach (var modifier in StatModifiers)
                {
                    Stat statRef = dataAccessibleOwner.GetStatReferer().GetStat(modifier.Key);
                    statRef.AddModifier(modifier.Value);
                    statRef.RecalculateStat();
                }
            }
            public void Run() { Timer.Execute(); }
            public void Revert(IDataAccessible dataAccessibleOwner)
            {
                foreach (var modifier in StatModifiers)
                {
                    Stat statRef = dataAccessibleOwner.GetStatReferer().GetStat(modifier.Key);
                    statRef.RemoveModifier(modifier.Value);
                    statRef.RecalculateStat();
                }
                OnRevert.Invoke();
            }

            #region User Interface 
            public string GetName() => Name;
            public string GetDescription() => Description;
            public Sprite GetSprite() => Icon;
            #endregion
        }
    }

    namespace Modifiers.Affector
    {
        /// <summary>
        /// Modifier이자, 일시적인 상태 변화를 야기하는 것.
        /// </summary>
        public abstract class Affector
        {
            #region Members
            public readonly CancellationTokenSource cts;
            public readonly E_AFFECT_TYPE AffectType; //
            public float BaseDurateTime { get; private set; } //
            protected Entity TargetRef; //
            protected Entity OwnerRef; //
            protected TimerComposite Timer { get; private set; } //
            public Affector(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime)
            {
                cts = new CancellationTokenSource();
                this.AffectType = affectType; //

                this.OwnerRef = ownerReceivers; //
                this.TargetRef = targetReceivers; //
                this.BaseDurateTime = durateTime; //

                this.Timer = new TimerComposite(BaseDurateTime); //

                OnModifiy = (ref float a) => { };
                OnTickRunning = () => { };
                OnRevert = () => { };
            }
            #endregion

            #region Setter

            public Affector SetAccelarationByTenacity(float tenecity)
            {
                Timer.SetAcceleratrion(tenecity);
                return this;
            }

            #endregion

            #region Event 

            public event UnityActionRef<float> OnModifiy;
            public event UnityAction OnTickRunning;
            public event UnityAction OnRevert;
            public event UnityAction OnCancle;
            public void ClearEvent()
            {
                OnModifiy = null;
                OnTickRunning = null;
                OnRevert = null;
                OnCancle = null;

                OnModifiy = (ref float a) => { };
                OnTickRunning = () => { };
                OnRevert = () => { };
                OnCancle = () => { };
            }
            #endregion

            public void ConveyToTarget() => TargetRef.ModifiedByAffector(this);
            public void Modifiy(float tenacity)
            {
                Timer.SetStart();
                Timer.Execute();
                OnModifiy?.Invoke(ref tenacity);
            }
            public void TickRunning()
            {
                Timer.Execute();
                OnTickRunning?.Invoke();
            }
            public void Revert() { OnRevert?.Invoke(); }

            public void CancleModify()
            {
                Timer.Pause();
                Timer.ChangeState(TimerExit.Instance);
                Timer = null;
                OnCancle?.Invoke();
                ClearEvent();
            }

            protected float CalcDurateTime(float tenacity) => BaseDurateTime * (1 - tenacity);
        }
    }
}
