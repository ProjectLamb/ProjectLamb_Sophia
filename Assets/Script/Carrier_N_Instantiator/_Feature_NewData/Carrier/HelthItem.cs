
using UnityEngine;

namespace Feature_NewData 
{
    public class HealthItem : MonoBehaviour {
        public int Health;

        public void SetHelth(int data) {
            Health = data;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player.Restore(Health);
            }
        }
    }
}