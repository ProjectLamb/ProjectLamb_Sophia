using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.Entitys;
    public class GearItem : MonoBehaviour{
        [SerializeField] public int Gear;

        public void SetGear(int data) => Gear = data;

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Player>(out Player player)){
                player._PlayerWealth.Gear += Gear;
            }
        }    
    }
}