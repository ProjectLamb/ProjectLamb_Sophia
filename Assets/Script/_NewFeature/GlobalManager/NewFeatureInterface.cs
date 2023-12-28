using UnityEngine;

namespace Feature_NewData
{
    public interface IDamagable {
        public void GetDamaged(int damage);
        public void GetDamaged(int damage, VFXObject vfx);
    }
    public interface IDieable {
        public void Die();

    }
//    public interface ILifeAccessable : IDamagable, IDieable{
//        public LifeComposite GetLifeComposite();
//    }

    public interface ILifeAccessable {
        public LifeComposite GetLifeComposite();
    }

    public interface ICarrierInteractable {
        public IStatAccessable GetStat();
    }
    public interface IStatAccessable {
        public Stat GetStat(E_NUMERIC_STAT_TYPE numericType);
        public string GetStatsInfo();
    }

    public interface ISkillStatAccessable
    {
        public Stat GetEfficienceMultiplyer();
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
    public interface IUpdatable
    {
        public void LateTick();
        public void FrameTick();
        public void PhysicsTick();
    
    }
}