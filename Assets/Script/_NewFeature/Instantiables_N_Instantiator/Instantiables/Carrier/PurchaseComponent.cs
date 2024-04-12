using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    public class PurchaseComponent : MonoBehaviour {
        public enum E_PURCHASE_TYPE { Equipment, Skill, HeartRate };
        public UnityAction OnPurchasedEvent;
        public UnityAction OnPurchasedDenyEvent;
        public int price;
        private void Awake() {
            OnPurchasedEvent        += () => {
                //Play purchase FMOD
                //VFX
            };
            OnPurchasedDenyEvent    += () => {
                //Play purchase deny FMOD
                //VFX
            };
        }
        public bool Purchase(Entitys.Player player)
        {
            int current_gear = player.PlayerWealth;
            if(current_gear >= this.price){
                player.PlayerWealth -= price;
                OnPurchasedEvent.Invoke();
                return true;
            }
            else {
                OnPurchasedDenyEvent.Invoke();
                return false;
            }
        }
        //private void OnTriggerEnter(Collider other) 
        //{
        //    if (!other.TryGetComponent<Player>(out Player player)) { return; }
        //    Purchase();
        //}
    }
}