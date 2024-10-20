using UnityEngine;

namespace Sophia.Instantiates 
{
    using FMODPlus;
    using Sophia.Entitys;
    public class GearItemObject : ItemObject {
        [SerializeField] public int Gear;
        public void SetGear(int data) => Gear = data;

        protected override void OnTriggerLogic(Collider entity)
        {
            if(!IsReadyToTrigger) {return;}
            if(entity.TryGetComponent<Player>(out Player player)){
                player.PlayerWealth += Gear;
                int WealthRefer = player.PlayerWealth;
                player.GetExtras<int>(E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered).PerformStartFunctionals(ref WealthRefer);
                _audioSource?.Play();
                IsReadyToTrigger = false;
                if(this._isDestroyable) {
                    Instantiate(_destroyEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerStay(Collider other) {
            OnTriggerLogic(other);    
        }
    }
}