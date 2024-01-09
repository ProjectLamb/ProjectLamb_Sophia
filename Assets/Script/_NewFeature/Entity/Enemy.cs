using System.Collections.Generic;

using UnityEngine;
using Sophia.Composite;
using Sophia.DataSystem.Numerics;
using Sophia.Instantiates;

namespace Sophia.Entitys{
    public abstract class Enemy : MonoBehaviour, IDamagable, IDieable, 
    // IStatAccessable, 
    IModelAccessable {

        public EntityStatReferer entityStat;
        public ModelManger modelManger;

        
        [SerializeField]
        private float currentHealth;

        private void Awake() {
            entityStat = new EntityStatReferer();
        }

        public void GetDamaged(int damage) {
            currentHealth -= damage;
            if(currentHealth <= 0) {Die();}
        }
        public void GetDamaged(int damage, VisualFXObject vfx) {}
        public void Die() {throw new System.NotImplementedException();}

        public void ChangeSkin(Material skin) { modelManger.ChangeSkin(skin); }
        public void RevertSkin() { modelManger.RevertSkin(); }
        public Animator GetAnimator() { return modelManger.GetAnimator(); }

    }    
}