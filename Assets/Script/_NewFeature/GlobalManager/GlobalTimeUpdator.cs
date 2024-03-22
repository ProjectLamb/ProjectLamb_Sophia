using System.Collections.Generic;
using UnityEngine;


namespace Sophia
{
    public class GlobalTimeUpdator : MonoBehaviour {
        public static HashSet<IUpdatable> TimerContainer = new();

        public static void CheckAndAdd(IUpdatable addingTimer){
            if(TimerContainer.TryGetValue(addingTimer, out IUpdatable finedTimer)) {return;}
            TimerContainer.Add(addingTimer);
        }

        public static void CheckAndRemove(IUpdatable removeingTimer) {
            TimerContainer.Remove(removeingTimer);
        }

        private void Update() {
            foreach(var timer in TimerContainer) {
                timer.FrameTick();
            }
        }
        private void LateUpdate() {
            foreach(var timer in TimerContainer) {
                timer.LateTick();
            }
        }

        private void FixedUpdate() {
            foreach(var timer in TimerContainer) {
                timer.PhysicsTick();
            }
        }
    }
}