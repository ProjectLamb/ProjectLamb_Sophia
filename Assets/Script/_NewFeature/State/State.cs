using UnityEngine;
using UnityEngine.Events;

namespace Feature_State
{
    public interface IState<T> {
        public void Enter(T owner);
        public void Execute(T owner);
        public void Exit(T owner);
    }
}