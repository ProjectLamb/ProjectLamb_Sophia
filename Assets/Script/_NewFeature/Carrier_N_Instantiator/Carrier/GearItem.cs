using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    public class GearItem : Carrier{
        [SerializeField] public int Gear;

        public void SetGear(int data) => Gear = data;

        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent<Player>(out Player player)){
                player._PlayerWealth.Gear += Gear;
            }
        }
    }
}