using UnityEngine;
    using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Sophia
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;

    public class GlobalEvent : MonoBehaviour, IDataSettable {

#region Member

        public Extras<object>               EnemyHit = null;
        public Extras<object>               EnemyDie = null;    
        public Extras<Stage>                StageClear = null;
        public Extras<(Stage, Stage)>       StageEnter = null;

#endregion

#region Event

        public UnityEvent                   OnEnemyDieEvent;
        public UnityEvent                   OnEnemyHitEvent;
        public UnityEvent<Stage>            OnStageClearEvent;
        public UnityEvent<Stage, Stage>     OnStageEnterEvent;
        private void OnEnemyHitUpdated()    => Debug.Log("몬스터 히트 동작 변경"); 
        private void OnEnemyDieUpdated()    => Debug.Log("몬스터 처치 동작 변경"); 
        private void OnStageClearUpdated()  => Debug.Log("스테이지 클리어 동작 변경");
        private void OnStageEnterUpdated()  => Debug.Log("스테이지 출입시 동작 변경");

#endregion

#region 
        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
        {
            extrasReferer.SetRefExtras<object>(EnemyHit);
            extrasReferer.SetRefExtras<object>(EnemyDie);
            extrasReferer.SetRefExtras<Stage>(StageClear);
            extrasReferer.SetRefExtras<(Stage,Stage)>(StageEnter);
            GlobalDataModelReferer.Instance.OnEnemyDie      = EnemyDie;
            GlobalDataModelReferer.Instance.OnEnemyHit      = EnemyHit;
            GlobalDataModelReferer.Instance.OnStageClear    = StageClear;
            GlobalDataModelReferer.Instance.OnStageEnter    = StageEnter;
        }

        public void SetStatDataToReferer(EntityStatReferer statReferer)
        {
            return;
        }

#endregion

        public void Init() {
            this.EnemyHit   = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyHit      , OnEnemyHitUpdated);
            this.EnemyDie   = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie      , OnEnemyDieUpdated);
            this.StageClear = new Extras<Stage>(E_FUNCTIONAL_EXTRAS_TYPE.StageClear    , OnStageClearUpdated);
            this.StageEnter = new Extras<(Stage, Stage)>(E_FUNCTIONAL_EXTRAS_TYPE.StageEnter    , OnStageEnterUpdated);
        }

        private void Awake() {Init();}

        private void Start() {}
    }
}