using UnityEngine;
using UnityEngine.Events;

namespace Sophia.CutScene {
    // Enter → Running         → Exit
    //         WaitingInput    ↗
    public class UICutSceneManager : MonoBehaviour, ICutScene
    {
        #region Serialized Member
        
        #endregion

        #region Events
        public event UnityAction OnEnterEvent;
        public event UnityAction OnRunEvent;
        public event UnityAction OnSkipEvent;
        public event UnityAction OnWaitEvent;
        public event UnityAction OnExitEvent;
        public event UnityAction OnEndEvent;
        #endregion

        private void Awake() {
            OnEnterEvent = null;
            OnExitEvent = null;
            OnRunEvent = null;
            OnSkipEvent = null;
            OnWaitEvent = null;
        }

        Coroutine CurrentRunningCoroutine;

        public E_CUTSCENE_STATE CurrentState { get; private set;}

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void End()
        {
            throw new System.NotImplementedException();
        }

        private void Start() {
            CurrentRunningCoroutine = null;
        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public void Wait()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
        public void Skip()
        {
            throw new System.NotImplementedException();
        }
    }
}