using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    public class GearItem : Carrier {
        [SerializeField] public int Gear;
        public bool triggeredOnce = false;
        public void SetGear(int data) => Gear = data;
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnTriggerLogic(Collider entity)
        {
            if(triggeredOnce) {return;}
            if(entity.TryGetComponent<Player>(out Player player)){

                player.PlayerWealth += Gear;
                int WealthRefer = player.PlayerWealth;
                player.GetExtras<int>(E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered).PerformStartFunctionals(ref WealthRefer);
                triggeredOnce = true;
                if(this._isDestroyable) {
                    Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}