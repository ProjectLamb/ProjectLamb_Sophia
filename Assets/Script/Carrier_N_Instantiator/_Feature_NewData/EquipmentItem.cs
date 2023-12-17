using UnityEngine;

namespace Feature_NewData 
{
    public class EquipmentItem : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if(other.GetComponent<Player>()) {
                
            }
        }
    }

}