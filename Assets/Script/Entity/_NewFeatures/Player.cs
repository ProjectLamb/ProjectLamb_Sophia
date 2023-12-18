using System.Runtime.InteropServices;
using UnityEngine;

namespace Feature_NewData
{
    public class Player 
        : MonoBehaviour, IRestoreable, IDamagable, IDieable, 
        IPlayerStatAccessable, IWeaponStatAccessable, IModelAccessable {
        
        private float currentHealth;
        private int CurrentStamina;
        public Stat StaminaRestoreSpeed;
        public Stat Luck;
        public Wealths PlayerWealth;
        public ModelManger modelManger;
        public MeleeWeapon meleeWeapon;

        public Stat GetMaxHP() { return MaxHp; }
        public Stat GetDefence() { return Defence; }
        public Stat GetPower() { return Power; }
        public Stat GetMoveSpeed() { return MoveSpeed; }
        public Stat GetTenacity() { return Tenacity; }
        public Stat GetMaxStamina() {return MaxStamina;}
        public Stat GetStaminaRestoreSpeed() {return StaminaRestoreSpeed;}
        public Stat GetLuck() {return Luck;}
        public Stat GetWeaponRatioDamage() { return meleeWeapon.GetWeaponRatioDamage();  }
        public Stat GetAttackSpeed() { return meleeWeapon.GetAttackSpeed();  }
        
        public IPlayerStatAccessable GetStat() {return this;}

        public void ChangeSkin(Material skin) { modelManger.ChangeSkin(skin); }
        public void RevertSkin() { modelManger.RevertSkin(); }
        public Animator GetAnimator() { return modelManger.GetAnimator(); }


        private void Awake() {
            MaxHp = new Stat(100, E_STAT_USE_TYPE.Natural);         
            Defence = new Stat(-1, E_STAT_USE_TYPE.Ratio);           // ?? 어떤 수치를 적용해야 하는걸까? 1~100?
            Power = new Stat(10, E_STAT_USE_TYPE.Natural);          // 0~inf
            MoveSpeed = new Stat(10, E_STAT_USE_TYPE.Natural);      // 초당 1칸을 음직이는 단위? 어떤 수치를 가지고, 
            Tenacity = new Stat(-1, E_STAT_USE_TYPE.Ratio);
            MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural);
            StaminaRestoreSpeed = new Stat(-1, E_STAT_USE_TYPE.Ratio);
            Luck = new Stat(0, E_STAT_USE_TYPE.Natural);
        }

        public void Die()
        {
            //OnDieEvent.Invoke(CurrentHealth);
        }

        public void GetDamaged(int damage)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0) {Die();}
            //OnGetDamaged.Invoke(CurrentHealth);
                //EX). UI 그려주기.
        }

        public void GetDamaged(int damage, VFXObject _obj)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0) {Die();}
            //OnGetDamaged.Invoke(CurrentHealth);
                //EX). UI 그려주기.
        }

        public void Dash() {
            if(CurrentStamina <= 0) return;
            CurrentStamina--;
        }

        public void Attack() {}
        public void Skill() {}
        public void Turning() {}

        public void Restore(int amount)
        {
            CurrentHealth += amount;
            if(CurrentHealth >= MaxHp.GetValueByNature()) {
                CurrentHealth = MaxHp.GetValueByNature();
            }
        }
    }    
}