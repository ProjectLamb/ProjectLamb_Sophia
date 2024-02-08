using UnityEngine;
using UnityEngine.Events;

namespace Feature_State
{
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