using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IDieable, IStatAccessable, IModelAccessable {

        public EntityStatManager entityStatManager;
        public ModelManger modelManger;

        
        [SerializeField]
        private float currentHealth;

        private void Awake() {
            entityStatManager = new EntityStatManager();
        }

        public void GetDamaged(int damage) {
            currentHealth -= damage;
            if(currentHealth <= 0) {Die();}
        }
        public void Die() {}

        public void ChangeSkin(Material skin) { modelManger.ChangeSkin(skin); }
        public void RevertSkin() { modelManger.RevertSkin(); }
        public Animator GetAnimator() { return modelManger.GetAnimator(); }

        public IStatAccessable GetStat()
        {
            return this;
        }

    }    
}