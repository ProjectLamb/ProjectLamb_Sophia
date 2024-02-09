using System.Collections.Generic;

using UnityEngine;

namespace Sophia.Entitys{
    using Cysharp.Threading.Tasks;
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;

    public abstract class Enemy : Entity, IMovable {

#region SerializeMember
        [SerializeField]
        private float currentHealth;

#endregion

#region Data Accessible
        public EntityStatReferer entityStat;

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType){
            return entityStat.GetStat(numericType);
        }

        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo() => entityStat.GetStatsInfo();


#endregion

#region Life Accessible
        public override bool GetDamaged(DamageInfo damage) {
            currentHealth -= damage.GetAmount();
            if(currentHealth <= 0) {Die();}
            return true;
        }
        public override bool Die() {throw new System.NotImplementedException();}

#endregion

        private void Awake() {
            entityStat = new EntityStatReferer();
        }


        public Animator GetAnimator() { return _modelManger.GetAnimator(); }

#region Move Accessible
        public bool GetMoveState()
        {
            throw new System.NotImplementedException();
        }

        public void SetMoveState(bool movableState)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTick()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Turning()
        {
            throw new System.NotImplementedException();
        }
#endregion
    }    
}