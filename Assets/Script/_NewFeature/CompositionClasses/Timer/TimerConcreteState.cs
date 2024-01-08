using UnityEngine;
using Feature_State;

namespace Sophia.Composite.Timer
{
    public enum E_TIMER_STATE
    {
        Initialized, Start, Timer, End, Terminate
    }

    public class TimerInitialize : IState<TimerComposite>
    {
        private static TimerInitialize _instance = new TimerInitialize();
        public static TimerInitialize Instance => _instance;

        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Initialized;
            owner.PassedTime = owner.BaseTime;
            owner.NextInterval = 0f;
            owner.OnInitialized?.Invoke();
        }

        public void Execute(TimerComposite owner) {return;}

        public void Exit(TimerComposite owner) { 
            owner.PassedTime = 0;
            return; 
        }
    }

    public class TimerStart : IState<TimerComposite>
    {
        private static TimerStart _instance = new TimerStart();
        public static TimerStart Instance => _instance;

        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Start;
            owner.OnStart?.Invoke();
        }

        public void Execute(TimerComposite owner)
        {
            owner.ChangeState(TimerRunning.Instance);
            return;
        }

        public void Exit(TimerComposite owner) {return;}
    }

    public class TimerRunning : IState<TimerComposite>
    {
        private static TimerRunning _instance = new TimerRunning();
        public static TimerRunning Instance => _instance;

        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Timer;
        }

        public void Execute(TimerComposite owner)
        {
            if (owner.PassedTime >= owner.BaseTime) { owner.ChangeState(TimerEnd.Instance); return; } // ChangeState(TimerEnd)
            if (owner.IntervalTime > 0.01f && owner.PassedTime >= owner.NextInterval)
            {
                owner.NextInterval += owner.IntervalTime;
                owner.OnInterval?.Invoke();
            }
            owner.OnTicking.Invoke(owner.GetProgressAmount());
            owner.PassedTime += owner.IsBlocked ? 0f : Time.deltaTime * owner.accelerationAmount;
        }

        public void Exit(TimerComposite owner) { return; }
    }

    public class TimerEnd : IState<TimerComposite>
    {
        private static TimerEnd _instance = new TimerEnd();
        public static TimerEnd Instance => _instance;

        public void Enter(TimerComposite owner)
        {
            owner.PassedTime = owner.BaseTime;
            owner.StateType = E_TIMER_STATE.End;
            owner.OnFinished?.Invoke();
            
            if (owner.WhenLoopable())
            {
                owner.PassedTime = 0;
                owner.NextInterval = 0f;
                owner.ChangeState(TimerStart.Instance);
            }
            else
            {
                owner.ChangeState(TimerInitialize.Instance);
            }
        }

        public void Execute(TimerComposite owner) { return; }

        public void Exit(TimerComposite owner) { return; }
    }

    public class TimerExit : IState<TimerComposite>
    {
        private static TimerExit _instance = new TimerExit();
        public static TimerExit Instance => _instance;
        public void Enter(TimerComposite owner)
        {
            owner.StateType = E_TIMER_STATE.Terminate;
        }

        public void Execute(TimerComposite owner) { return; }

        public void Exit(TimerComposite owner) { return; }
    }
}