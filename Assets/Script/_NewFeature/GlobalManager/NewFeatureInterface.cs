using UnityEngine;


namespace Sophia
{
    using Instantiates;
    using Composite;
    using Sophia.DataSystem.Modifiers;

    /*********************************************************************************
    * IUpdatorBindable & IUpdatable
    * 이 두가지는 MonoBehaviour을 상속받지 않는 애들임에도
    * 이 클래스를 구현한 애들은 Update 시간마다 계산이 필요할때 사용
    * 모노비헤이비어에 연결 될 필요는 없는데. 지속적인 계산이 필요한 녀석들

    * 대표적으로 물리계산이 필요한 애들.
    *********************************************************************************/
    public interface IUpdatorBindable : IUpdatable {
        public bool GetUpdatorBind();
        public void AddToUpdater();
        public void RemoveFromUpdator();
    }

    public interface IUpdatable
    {
        public void LateTick();
        public void FrameTick();
        public void PhysicsTick();
    
    }

//    public interface ICarrierObjectInteractable {
//        public IStatAccessible GetStat();
//    }

//     public interface ISkillStatAccessible
//     {
//         public Stat GetEfficienceMultiplyer();
//     }

    public interface IInstantiatorAccessible {
        public ProjectileBucketManager GetProjectileBucketManager();
    }

//     public interface IWeaponStatAccessible {
//         public Stat GetWeaponRatioDamage();
//         public Stat GetAttackSpeed();
//     }

//    public interface IRestoreable {
//        public void Restore(int amount);
//    }

    public interface IVisualAccessible {
        public Composite.RenderModels.ModelManager GetModelManager();
        public VisualFXBucket GetVisualFXBucket();
    }


//     public interface IUseMonobehaviourConstructor {
//         public void Initialize(object data);
//     }

    public interface IAttackable
    {
        public void Attack();
    }

    public interface IWeaponManagerAccessible : IAffectable {
        public WeaponManager GetWeaponManager();
    }

    public interface IAudioAccessible
    {
        public EntityAudioManager GetAudioManager();
    }
}