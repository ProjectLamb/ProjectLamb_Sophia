using UnityEngine;

namespace Sophia.DataSystem
{
    using System;
    using Sophia.Composite.Timer;
    using Sophia.Entitys;
    using Sophia.Instantiates;
    using UnityEngine.Events;

    namespace Modifiers.Affector
    {
        public abstract class Affector
        {
            #region Members
            public readonly E_AFFECT_TYPE AffectType;
            public readonly Entity targetEntity;
            public readonly Entity ownerEntity;
            public float BaseDurateTime { get; private set; }
            public TimerComposite Timer { get; private set; }
            public Affector(E_AFFECT_TYPE affectType, Entity ownerReceivers, Entity targetReceivers, float durateTime)
            {
                this.AffectType = affectType;

                this.ownerEntity = ownerReceivers;
                this.targetEntity = targetReceivers;
                this.BaseDurateTime = durateTime;

                this.Timer = new TimerComposite(BaseDurateTime);

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

                OnModifiy = (ref float a) => {};
                OnTickRunning = () => {};
                OnRevert = () => {};
                OnCancle = () => {};
            }
            #endregion

            public virtual void ConveyToTarget() => targetEntity.ModifiedByAffector(this);
            public virtual void Modifiy(float tenacity) { OnModifiy?.Invoke(ref tenacity); }
            public virtual void TickRunning() { OnTickRunning?.Invoke(); }
            public virtual void Revert() { OnRevert?.Invoke(); }
            public virtual void CancleModify()
            {
                Timer = null;
                OnCancle?.Invoke();
                ClearEvent();
            }
        }
    }
}