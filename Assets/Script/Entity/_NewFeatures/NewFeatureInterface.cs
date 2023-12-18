using UnityEngine;

namespace Feature_NewData
{
    public interface IDamagable {
        public void GetDamaged(int damage);
    }
    public interface IDieable {
        public void Die();

    }
    public interface ICarrierInteractable {
        public IStatAccessable GetStat();
    }
    public interface IStatAccessable {
        public Stat GetMaxHP();
        public Stat GetDefence();
        public Stat GetPower();
        public Stat GetMoveSpeed();
        public Stat GetTenacity();
    }

    public interface ISkillStatAccessable
    {
        public Stat GetSkillEffectMultiplyer();
    }

    public interface IPlayerStatAccessable : IStatAccessable {
        public Stat GetMaxStamina();
        public Stat GetStaminaRestoreSpeed();
        public Stat GetLuck();
    }

    public interface IInstantiatorAccessable {

    }

    public interface IWeaponStatAccessable {
        public Stat GetWeaponRatioDamage();
        public Stat GetAttackSpeed();
    }

    public interface ICarrierInstantiator {
        public void InstanceCarrier();
    }

    public interface IRestoreable {
        public void Restore(int amount);
    }

    public interface IModelAccessable {
        public void ChangeSkin(Material skin);
        public void RevertSkin();
        public Animator GetAnimator();
    }

    public interface IUseMonobehaviourConstructor {
        public void Initialize(object data);
    }
}