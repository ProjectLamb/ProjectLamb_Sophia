using UnityEngine;
using UnityEngine.Events;

namespace Sophia.DataSystem.Modifiers
{
    using System;
    using System.Threading;
    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;
    using Sophia.State;

    public abstract class Affector : IStateMachine<AffectorState, Entitys.Entity>, ITimerAccessible<Entity>
    {
#region Members

        public E_AFFECT_TYPE AffectType {get; protected set;}
        public string Name {get; protected set;}
        public string Description {get; protected set;}
        public Sprite Icon {get; protected set;}
        protected TimerComposite Timer;

        public Affector(in SerialAffectorData affectData)
        {
            this.Init(in affectData);
            OnClear += ResetState;
        }

        protected abstract void Init(in SerialAffectorData affectData);
#endregion

#region Event

        public event UnityAction<Affector> OnClear;
        public void ClearAffect(Affector affector) => OnClear?.Invoke(affector);


#endregion

#region State Machine

        protected AffectorState CurrentState;
        public AffectorState GetCurrentState() => CurrentState;
        public void ChangeState(AffectorState newState) {
            if(newState == null) return;
            if(GetIstransferableState(newState)) CurrentState = newState;
        }
        public void ExecuteState(Entity entity) => CurrentState.Affect(this, entity);
        public bool GetIstransferableState(AffectorState transState) {
            return (CurrentState.GetTransitionBit() & transState.GetCurrentBit()) == transState.GetCurrentBit();
        }
        public void ResetState(Affector affector) => ResetState();
        public void ResetState() {
            this.Timer.ResetTimer();
            this.CurrentState = AffectorReadyState.Instance;
            this.OnClear += ResetState;
        }

#endregion
        
#region Timer

        public TimerComposite GetTimerComposite() => Timer;
        public abstract void Enter(Entity entity);
        public abstract void Run(Entity entity);
        public virtual void Exit(Entity entity) { OnClear?.Invoke(this); }

#endregion

    }

    public interface AffectorState : ITransitionAccessible{
        public void Affect(Affector affector, Entity entity);
    }

    public class AffectorReadyState : AffectorState
    {
        private static AffectorReadyState _instance = new AffectorReadyState();
        public static AffectorReadyState Instance => _instance;
        public void Affect(Affector affector, Entity entity)
        {
            affector.ChangeState(AffectorStartState.Instance);
        }

        public int GetCurrentBit() => (int)TimerStateBit.Ready;
        public int GetTransitionBit() => (int)TimerStateBit.Start;
    }

    public class AffectorStartState : AffectorState
    {
        private static AffectorStartState _instance = new AffectorStartState();
        public static AffectorStartState Instance => _instance;

        public void Affect(Affector affector, Entity entity)
        {
            affector.Enter(entity);
            affector.ChangeState(AffectorRunState.Instance);
        }

        public int GetCurrentBit() => (int)TimerStateBit.Start;
        public int GetTransitionBit() => (int)TimerStateBit.Run;
    }

    public class AffectorRunState : AffectorState
    {
        private static AffectorRunState _instance = new AffectorRunState();
        public static AffectorRunState Instance => _instance;

        public void Affect(Affector affector, Entity entity)
        {
            if(affector.GetTimerComposite().IsBlocked)      { affector.ChangeState(AffectorPauseState.Instance);        return;}
            if(affector.GetTimerComposite().GetIsTimesUp()) { affector.ChangeState(AffectorTerminateState.Instance);    return;}
            if(affector.GetTimerComposite().GetIsActivateInterval()) { 
                affector.Run(entity);
            }
        }

        public int GetCurrentBit() => (int)TimerStateBit.Run;
        public int GetTransitionBit() => (int)TimerStateBit.Terminate + (int)TimerStateBit.Pause;
    }

    public class AffectorPauseState : AffectorState
    {
        private static AffectorPauseState _instance = new AffectorPauseState();
        public static AffectorPauseState Instance => _instance;
        public void Affect(Affector affector, Entity entity)
        {
            if(!affector.GetTimerComposite().IsBlocked) {affector.ChangeState(AffectorRunState.Instance); return;}
        }

        public int GetCurrentBit() => (int)TimerStateBit.Pause;
        public int GetTransitionBit() => (int)TimerStateBit.Terminate + (int)TimerStateBit.Run;
    }

    public class AffectorTerminateState : AffectorState
    {
        private static AffectorTerminateState _instance = new AffectorTerminateState();
        public static AffectorTerminateState Instance => _instance;
        public void Affect(Affector affector, Entity entity)
        {
            affector.Exit(entity);
        }

        public int GetCurrentBit() => (int)TimerStateBit.Terminate;
        public int GetTransitionBit() => 0;
    }
    
}
