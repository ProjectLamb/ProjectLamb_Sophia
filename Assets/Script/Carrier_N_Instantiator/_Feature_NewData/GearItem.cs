using UnityEngine;

namespace Feature_NewData 
{
    public class GearItem : MonoBehaviour {
        public int Gear;

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player.PlayerWealth.Gear += Gear;
            }
        }
    }
}