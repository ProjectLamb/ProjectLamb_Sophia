using UnityEngine;
using UnityEngine.Events;

namespace Sophia.State
{

    public enum TimerStateBit { End = 0, Ready = 1 << 0, Start = 1 << 1, Run = 1 << 2, Pause = 1 << 3, Terminate = 1 << 4 }
    public interface IStateMachine<State, Receiver> {
        public State GetCurrentState();
        public void ChangeState(State newState);
        public void ExecuteState(Receiver receiver);
        public void ResetState();
        public bool GetIstransferableState(State transState);
    }

    public interface IState<T> {
        public void Enter(T owner);
        public void Execute(T owner);
        public void Exit(T owner);
    }

    public interface ITransitionAccessible {
        public int GetTransitionBit();
        public int GetCurrentBit();
    }

    public interface IAffectState<T> : ITransitionAccessible {
        public void Enter(T manager, Sophia.Entitys.Entity entity);
        public void Execute(T manager, Sophia.Entitys.Entity entity);
        public void Exit(T manager, Sophia.Entitys.Entity entity);
    }
}