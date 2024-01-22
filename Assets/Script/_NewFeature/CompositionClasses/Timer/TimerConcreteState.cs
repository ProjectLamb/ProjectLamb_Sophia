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

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Initialized;
            timer.PassedTime = timer.BaseTime;
            timer.NextInterval = 0f;
            timer.InvoekInitializedAction();
        }

        public void Execute(TimerComposite timer) {return;}

        public void Exit(TimerComposite timer) { 
            timer.PassedTime = 0;
            return; 
        }
    }

    public class TimerStart : IState<TimerComposite>
    {
        private static TimerStart _instance = new TimerStart();
        public static TimerStart Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Start;
            timer.InvokeStartAction();
            timer.ChangeState(TimerRunning.Instance);
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) {return;}
    }

    public class TimerRunning : IState<TimerComposite>
    {
        private static TimerRunning _instance = new TimerRunning();
        public static TimerRunning Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Timer;
        }

        public void Execute(TimerComposite timer)
        {
            if (timer.PassedTime >= timer.BaseTime) { timer.ChangeState(TimerEnd.Instance); return; } // ChangeState(TimerEnd)
            if (timer.IntervalTime > 0.01f && timer.PassedTime >= timer.NextInterval)
            {
                timer.NextInterval += timer.IntervalTime * timer.accelerationAmount;
                timer.InvokeIntervalAction();
            }
            timer.InvokeTickingAction(timer.GetProgressAmount());
            timer.PassedTime += timer.IsBlocked ? 0f : Time.deltaTime * timer.accelerationAmount;
        }
        
        public void Exit(TimerComposite timer) { return; }
    }

    public class TimerEnd : IState<TimerComposite>
    {
        private static TimerEnd _instance = new TimerEnd();
        public static TimerEnd Instance => _instance;

        public void Enter(TimerComposite timer)
        {
            timer.PassedTime = timer.BaseTime;
            timer.StateType = E_TIMER_STATE.End;
            timer.InvokeFinishedAction();
            
            if (timer.GetIsRewindable())
            {
                timer.PassedTime = 0;
                timer.NextInterval = 0f;
                timer.ChangeState(TimerStart.Instance);
            }
            else
            {
                timer.ChangeState(TimerInitialize.Instance);
            }
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) { return; }
    }

    public class TimerExit : IState<TimerComposite>
    {
        private static TimerExit _instance = new TimerExit();
        public static TimerExit Instance => _instance;
        public void Enter(TimerComposite timer)
        {
            timer.StateType = E_TIMER_STATE.Terminate;
        }

        public void Execute(TimerComposite timer) { return; }

        public void Exit(TimerComposite timer) { return; }
    }
}