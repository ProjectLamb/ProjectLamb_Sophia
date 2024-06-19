using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.CutScene {
    public interface ICutScene {
        public E_CUTSCENE_STATE CurrentState { get; }
        public void Init();
        public void Enter();
        public void Run();
        public void Wait();
        public void Exit();
        public void Skip();
        public void End();
        public event UnityAction OnEnterEvent;
        public event UnityAction OnRunEvent;
        public event UnityAction OnSkipEvent;
        public event UnityAction OnWaitEvent;
        public event UnityAction OnExitEvent;
        public event UnityAction OnEndEvent;
    }
}