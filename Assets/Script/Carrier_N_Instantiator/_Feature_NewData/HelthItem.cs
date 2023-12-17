
using UnityEngine;

namespace Feature_NewData 
{
    public class HealthItem : MonoBehaviour {
        public int Health;

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player.Restore(Health);
            }
        }
    }
}