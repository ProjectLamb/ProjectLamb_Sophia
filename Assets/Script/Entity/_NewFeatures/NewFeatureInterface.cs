using UnityEngine;

namespace Feature_NewData
{
    public interface IDamagable {
        public void GetDamaged(int damage);
    }
    public interface IDieable {

    }
    public interface ICarrierInteractable {
        public IStatAccessable GetStat();
    }
    public interface IStatAccessable {
        public Stat GetMaxUp();
        public Stat GetDefence();
        public Stat GetPower();
        public Stat GetMoveSpeed();
        public Stat GetTenacity();
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
}