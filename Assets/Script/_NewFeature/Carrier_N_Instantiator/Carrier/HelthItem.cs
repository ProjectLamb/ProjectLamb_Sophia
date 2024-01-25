
using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    
    public class HealthItem : Carrier {
        [SerializeField] public int Health;

        public void SetHelth(int data) => Health = data;

        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent<Player>(out Player player)){
                player.Life.Healed(Health);
            }
        }
    }
}