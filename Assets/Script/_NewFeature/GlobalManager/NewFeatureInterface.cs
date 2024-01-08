using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Sophia
{

    public interface IVisualAccessable {
        
    }

    /*********************************************************************************
    * IUpdatorBindable & IUpdatable
    * 이 두가지는 MonoBehaviour을 상속받지 않는 애들임에도
    * 이 클래스를 구현한 애들은 Update 시간마다 계산이 필요할때 사용
    * 모노비헤이비어에 연결 될 필요는 없는데. 지속적인 계산이 필요한 녀석들

    * 대표적으로 물리계산이 필요한 애들.
    *********************************************************************************/
    public interface IUpdatorBindable : IUpdatable {
        public bool GetUpdatorBind();
        public void AddToUpator();
        public void RemoveFromUpdator();
    }

    public interface IUpdatable
    {
        public void LateTick();
        public void FrameTick();
        public void PhysicsTick();
    
    }

//    public interface ICarrierInteractable {
//        public IStatAccessable GetStat();
//    }

//     public interface ISkillStatAccessable
//     {
//         public Stat GetEfficienceMultiplyer();
//     }

    public interface IInstantiatorAccessable {

    }

//     public interface IWeaponStatAccessable {
//         public Stat GetWeaponRatioDamage();
//         public Stat GetAttackSpeed();
//     }

//    public interface IRestoreable {
//        public void Restore(int amount);
//    }

    public interface IModelAccessable {
        public void ChangeSkin(Material skin);
        public void RevertSkin();
        public Animator GetAnimator();
    }

//     public interface IUseMonobehaviourConstructor {
//         public void Initialize(object data);
//     }
}