using BehaviorDesigner.Runtime.Tasks.Unity.UnityParticleSystem;
using Sophia.Composite;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Entitys
{
    public class DataMadWaiter : IUpdatable
    {
        private readonly TimerComposite timer;
        
        public DataMadWaiter(float baseTime)
        {
            timer = new TimerComposite(baseTime).SetIntervalTime(1f);
            Debug.Log($"{timer.BaseTime} 초로 설정");
        }

        public void ActionStart()
        {
            timer.SetStart();
            timer.Execute();
        }

        public DataMadWaiter AddOnIntervalEvent(UnityAction action)
        {
            timer.OnInterval += action;
            return this;
        }

        public DataMadWaiter AddOnTickingEvent(UnityAction<float> action)
        {
            timer.OnTicking += action;
            return this;
        }

        public DataMadWaiter AddOnFinishedEvent(UnityAction action)
        {
            timer.OnFinished += action;
            return this;
        }

        public void LateTick()
        {
            timer.Execute();
        }

        public void FrameTick(){return;}

        public void PhysicsTick() { return; }
    }
}