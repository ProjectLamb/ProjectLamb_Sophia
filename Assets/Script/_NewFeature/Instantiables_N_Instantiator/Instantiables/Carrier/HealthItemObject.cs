
using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    using UnityEngine.VFX;

    public class HealthItemObject : ItemObject {
        [SerializeField] public int Health;
        public void SetHealth(int data) => Health = data;

        protected override void OnTriggerLogic(Collider entity)
        {
            if(!IsReadyToTrigger) {return;}
            if(entity.TryGetComponent<Player>(out Player player)){
                player.GetLifeComposite().Healed(Health);
                
                _lootVFX.Stop();
                _lootObject.SetActive(false);
                IsReadyToTrigger = false;
                if(this._isDestroyable) Destroy(gameObject, 2f);
            }
        }
    }
}