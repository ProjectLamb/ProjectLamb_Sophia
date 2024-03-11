using UnityEngine;
using Cysharp.Threading.Tasks;
using Sophia.DataSystem;
using UnityEngine.Events;

namespace Sophia
{
    public class GlobalDataModelReferer : MonoBehaviour {
        private static GlobalDataModelReferer _instance;
        public static GlobalDataModelReferer Instance {
            get {
                if(_instance == null) {
                    _instance = FindFirstObjectByType(typeof(GlobalDataModelReferer)) as GlobalDataModelReferer;
                    if(_instance == null) Debug.Log("no Singleton obj");
                }
                return _instance;
            }
            private set {}
        }

#region Player


#endregion

#region Stage
        
        public Extras<object>               OnEnemyHit      = null;
        public Extras<object>               OnEnemyDie      = null;    
        public Extras<Stage>                OnStageClear    = null;
        public Extras<(Stage, Stage)>       OnStageEnter    = null;

#endregion

#region System

#endregion
    }
}