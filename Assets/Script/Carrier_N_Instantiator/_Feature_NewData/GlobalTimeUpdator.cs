using System.Collections.Generic;
using UnityEngine;


namespace Feature_NewData
{
    public class GlobalTimeUpdator : MonoBehaviour {
        public static HashSet<IGlobalGameTimeUpdator> TimerContainer = new();

        public static void CheckAndAdd(IGlobalGameTimeUpdator addingTimer){
            if(TimerContainer.TryGetValue(addingTimer, out IGlobalGameTimeUpdator finedTimer)) {return;}
            TimerContainer.Add(addingTimer);
        }

        public static void CheckAndRemove(IGlobalGameTimeUpdator removeingTimer) {
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

        private void PhysicsTick() {
            foreach(var timer in TimerContainer) {
                timer.PhysicsTick();
            }
        }
    }
}