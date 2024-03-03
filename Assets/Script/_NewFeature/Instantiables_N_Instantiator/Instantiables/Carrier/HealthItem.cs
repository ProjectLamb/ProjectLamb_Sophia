
using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    
    public class HealthItem : Carrier {
        [SerializeField] public int Health;
        public bool triggeredOnce = false;
        public void SetHealth(int data) => Health = data;
        protected override void Awake() {
            base.Awake();
        }
        protected override void OnTriggerLogic(Collider entity)
        {
            if(triggeredOnce) {return;}
            if(entity.TryGetComponent<Player>(out Player player)){
                player.GetLifeComposite().Healed(Health);
                triggeredOnce = true;
                if(this._isDestroyable) Destroy(gameObject, 1f);
            }
        }
    }
}