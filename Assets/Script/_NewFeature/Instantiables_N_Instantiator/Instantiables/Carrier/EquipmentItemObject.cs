using UnityEngine.VFX;
using UnityEngine;

namespace Sophia.Instantiates
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.Entitys;
    using Sophia.UserInterface;

    public class EquipmentItemObject : ItemObject
    {
        [SerializeField] SerialEquipmentData _equipmentData;
        public Equipment equipment { get; private set; }        
        public bool ISDEBUG = true;

        private void Start() {
            if(ISDEBUG) {DEBUG_Activate();}
        }


        protected override void OnTriggerLogic(Collider entity)
        {
            if(!IsReadyToTrigger) return;
            if (entity.TryGetComponent(out Player player))
            {
                equipment = FactoryConcreteEquipment.GetEquipmentByID(in _equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                if (EquipUserInterface())
                {
                    player.EquipEquipment(equipment);

                    _lootVFX.Stop();
                    _lootObject.SetActive(false);
                    IsReadyToTrigger = false;
                    if(this._isDestroyable) Destroy(this.gameObject, 2);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }

}
