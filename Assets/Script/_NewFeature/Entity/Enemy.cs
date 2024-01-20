using System.Collections.Generic;

using UnityEngine;

namespace Sophia.Entitys{
    using Cysharp.Threading.Tasks;
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Numerics;
    using Sophia.Instantiates;

    public abstract class Enemy : Entity, IModelAccessable {

        public EntityStatReferer entityStat;
        
        [SerializeField]
        private float currentHealth;

        private void Awake() {
            entityStat = new EntityStatReferer();
        }

        public override void GetDamaged(int damage) {
            currentHealth -= damage;
            if(currentHealth <= 0) {Die();}
        }
        public override void GetDamaged(int damage, VisualFXObject vfx) {}
        public override void Die() {throw new System.NotImplementedException();}

        public void ChangeSkin(Material skin) { _modelManger.ChangeSkin(skin).Forget(); }
        public void RevertSkin() { _modelManger.RevertSkin().Forget(); }
        public Animator GetAnimator() { return _modelManger.GetAnimator(); }

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType){
            return entityStat.GetStat(numericType);
        }

        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo() => entityStat.GetStatsInfo();
    }    
}