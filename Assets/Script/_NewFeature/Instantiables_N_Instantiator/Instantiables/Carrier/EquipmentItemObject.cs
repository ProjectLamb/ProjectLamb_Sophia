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
        [SerializeField] PurchaseComponent _purchaseComponent;
        public SerialEquipmentData GetSerialEquipmentData() => _equipmentData;
        public Equipment equipment { get; private set; }
        public bool ISDEBUG = true;

        private void Start()
        {
            if (ISDEBUG) { DEBUG_Activate(); }
        }


        protected override void OnTriggerLogic(Collider entity)
        {
            if (!IsReadyToTrigger) return;
            if (entity.TryGetComponent(out Player player))
            {
                if (TryGetComponent<PurchaseComponent>(out _purchaseComponent))
                {
                    if(!_purchaseComponent.Purchase(player)) return;
                }
                equipment = FactoryConcreteEquipment.GetEquipmentByID(in _equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                if (EquipUserInterface())
                {
                    player.EquipEquipment(equipment);

                    this._lootVFX.playRate *= 2;
                    _lootVFX.Stop();
                    _lootObject.SetActive(false);
                    IsReadyToTrigger = false;

                    //UI
                    InGameScreenUI.Instance._equipmentDescriptionUI.Init(_equipmentData._equipmentName, _equipmentData._description);
                    InGameScreenUI.Instance._equipmentDescriptionUI.DisplayOn();

                    if (this._isDestroyable) Destroy(this.gameObject, 2);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }

}
