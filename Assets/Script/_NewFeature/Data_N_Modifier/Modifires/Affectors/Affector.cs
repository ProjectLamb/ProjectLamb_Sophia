using UnityEngine;
using UnityEngine.Events;

namespace Sophia.DataSystem.Modifiers
{
    using Sophia.Composite.NewTimer;
    using Sophia.Entitys;


    public abstract class Affector : IUserInterfaceAccessible
    {
        #region Members

        public readonly E_AFFECT_TYPE AffectType;
        public readonly string Name;
        public readonly string Description;
        public readonly Sprite Icon;
        public TimerComposite Timer {get; protected set;}

        public Affector(SerialAffectorData affectData)
        {
            AffectType = affectData._affectType;
            Name = affectData._equipmentName;
            Description = affectData._description;
            Icon = affectData._icon;
        }
        #endregion

        #region Event
        public event UnityAction OnRevert;
        protected void InvokeOnRevertAffect() => OnRevert();

        #endregion

        #region State
        public AffectorState CurrentState;
        public void ChangeState(AffectorState newState) {
            if(newState == null) return;
            CurrentState = newState;
        }
        public void Affect(Entity entity) => CurrentState.Affect(this, entity);

        #endregion

        public abstract void Invoke(Entity entity);
        public abstract void Run(Entity entity);
        public abstract void Revert(Entity entity);

        #region User Interface 
        
        public string GetName() => Name;
        public string GetDescription() => Description;
        public Sprite GetSprite() => Icon;

        #endregion
    }

    public interface IStateMachine<State> {
        public void ChangeState(State newState);
    }

    public interface ITimerState<Receiver> {
        public void Execute(IStateMachine<ITimerState<Receiver>> machine, Receiver receiver);
    }


    public interface AffectorState {
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
    }

    public class AffectorStartState : AffectorState
    {
        private static AffectorStartState _instance = new AffectorStartState();
        public static AffectorStartState Instance => _instance;

        public void Affect(Affector affector, Entity entity)
        {
            affector.Invoke(entity);
            affector.ChangeState(AffectorRunState.Instance);
        }
    }

    public class AffectorRunState : AffectorState
    {
        private static AffectorRunState _instance = new AffectorRunState();
        public static AffectorRunState Instance => _instance;

        public void Affect(Affector affector, Entity entity)
        {
            if(affector.Timer.GetIsActivateInterval()) affector.Run(entity);
            if(affector.Timer.GetIsTimesUp()) affector.ChangeState(AffectorTerminateState.Instance);
        }
    }

    public class AffectorTerminateState : AffectorState
    {
        private static AffectorTerminateState _instance = new AffectorTerminateState();
        public static AffectorTerminateState Instance => _instance;
        public void Affect(Affector affector, Entity entity)
        {
            affector.Revert(entity);
        }
    }

}
