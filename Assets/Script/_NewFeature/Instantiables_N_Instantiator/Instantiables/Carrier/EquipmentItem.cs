using UnityEngine.VFX;
using UnityEngine;

namespace Sophia.Instantiates
{
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.Entitys;

    public class EquipmentItem : Carrier
    {
        [SerializeField] public GameObject lootObject;
        [SerializeField] public VisualEffect lootVFX;
        [SerializeField] SerialEquipmentData _equipmentData;
        public Equipment equipment { get; private set; }
        
        public bool triggeredOnce = false;

        protected override void Awake()
        {
            base.Awake();
            equipment = FactoryConcreteEquipment.GetEquipmentByID(in _equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if(triggeredOnce) return;
            if (entity.TryGetComponent(out Player player))
            {
                if (EquipUserInterface())
                {
                    player.EquipEquipment(equipment);

                    lootVFX.Stop();
                    lootObject.SetActive(false);
                    triggeredOnce = true;
                    if(this._isDestroyable) Destroy(this.gameObject, 3);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }

}
