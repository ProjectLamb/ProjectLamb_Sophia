using UnityEngine;

namespace Sophia.Instantiates 
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.Entitys;
    public class EquipmentItem : Carrier
    {
        [SerializeField] SerialEquipmentData _equipmentData;
        public Equipment equipment {get; private set;}

        private void Awake() {
            equipment = FactoryConcreteEquipment.GetEquipmentByID(ref _equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if (entity.TryGetComponent(out Player player))
            {
                if(EquipUserInterface()){
                    player.Equip(equipment);
                    Destroy(this.gameObject);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }
}
