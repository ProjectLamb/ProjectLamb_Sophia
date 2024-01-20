
using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    
    public class HealthItem : MonoBehaviour {
        [SerializeField] public int Health;

        public void SetHelth(int data) => Health = data;

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player.Life.Healed(Health);
            }
        }
    }
}