using System.Collections;
using UnityEngine;

namespace TEST
{
    //https://www.youtube.com/watch?v=5PTd0WdKB-4
    public abstract class Begin : State 
    {
        
        public override IEnumerator Start() {
            yield return null;
        }
        public override IEnumerator Attack() {
            yield return null;
        }
        public override IEnumerator Heal() {
            yield return null;
        }
    }
    public abstract class PlayerTurn : State 
    {
        
        public override IEnumerator Start() {
            yield return null;
        }
        public override IEnumerator Attack() {
            yield return null;
        }
        public override IEnumerator Heal() {
            yield return null;
        }
    }
}
