using System.Collections;
using UnityEngine;

namespace TEST
{
    //https://www.youtube.com/watch?v=5PTd0WdKB-4
    public abstract class State 
    {
        protected BattleSyetem battleSyetem;
        public abstract IEnumerator Start();
        public abstract IEnumerator Attack();
        public abstract IEnumerator Heal();
    }
}
