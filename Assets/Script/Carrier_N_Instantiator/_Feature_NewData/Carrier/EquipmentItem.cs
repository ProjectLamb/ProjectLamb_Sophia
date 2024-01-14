using UnityEngine;

namespace Feature_NewData 
{
    public class EquipmentItem : MonoBehaviour {
        private Equipment equipmentRef;
        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Player>()) {
                
            }
        }

        public void SetEquipment(Equipment equipment) {
            equipmentRef = equipment;
        }
    }
}