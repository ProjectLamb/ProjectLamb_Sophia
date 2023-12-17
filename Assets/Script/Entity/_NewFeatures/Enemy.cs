using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IDieable, IStatAccessable, IModelAccessable {
        public Stat MaxUp;
        private float CurrentHealth;
        public Stat Defence;
        public Stat Power;
        public Stat AttackSpeed;
        public Stat MoveSpeed;
        public Stat Tenacity;

        public ModelManger modelManger;

        public void GetDamaged(int damage)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0) {Die();}
        }

        public void GetDamaged(int damage, VFXObject _obj)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0) {Die();}
        }

        public void ChangeSkin(Material skin) { modelManger.ChangeSkin(skin); }
        public void RevertSkin() { modelManger.RevertSkin(); }
        public Animator GetAnimator() { return modelManger.GetAnimator(); }

        public void Die()
        {
        }
        public Stat GetMaxUp() { return MaxUp; }
        public Stat GetDefence() { return Defence; }
        public Stat GetPower() { return Power; }
        public Stat GetAttackSpeed() { return AttackSpeed; }
        public Stat GetMoveSpeed() { return MoveSpeed; }
        public Stat GetTenacity() { return Tenacity; }

        public IStatAccessable GetStat()
        {
            return this;
        }

    }    
}