using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
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
        public Equipment equipment { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            equipment = FactoryConcreteEquipment.GetEquipmentByID(in _equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if (entity.TryGetComponent(out Player player))
            {
                if (EquipUserInterface())
                {
                    player.EquipEquipment(equipment);
                    Destroy(this.gameObject);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }

}
