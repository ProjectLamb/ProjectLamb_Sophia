using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    public class GearItem : MonoBehaviour {
        public int Gear;

        public void SetGear(int data) {
            Gear = data;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player.PlayerWealth.Gear += Gear;
            }
        }
    }
}