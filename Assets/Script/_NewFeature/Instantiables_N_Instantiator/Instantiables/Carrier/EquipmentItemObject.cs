using Sophia.DB;
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
        [SerializeField] ScriptableEquipmentData _equipmentData;
        [SerializeField] PurchaseComponent _purchaseComponent;
        public IEquipmentDataAccessable GetSerialEquipmentData() => _equipmentData;
        public Equipment equipment { get; private set; }
        public bool ISDEBUG = true;

        private void Start()
        {
            if (ISDEBUG) { DEBUG_Activate(); }
            Invoke("StopVFX", 1f);
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if (!IsReadyToTrigger) return;
            if (entity.TryGetComponent(out Player player))
            {
                if (TryGetComponent<PurchaseComponent>(out _purchaseComponent))
                {
                    if (!_purchaseComponent.Purchase(player)) return;
                }
                equipment = FactoryConcreteEquipment.GetEquipmentByID(_equipmentData, GameManager.Instance.PlayerGameObject.GetComponent<Player>());
                if (EquipUserInterface())
                {
                    player.EquipEquipment(equipment);

                    //File
                    DontDestroyGameManager.Instance.SaveLoadManager.Data.PlayerData.CollectedEquipmentIndexs.Add(_equipmentData.EquipmentID);

                    //_lootVFX.Stop();
                    _lootObject.SetActive(false);
                    IsReadyToTrigger = false;

                    //UI
                    InGameScreenUI.Instance._equipmentDescriptionUI.Init(_equipmentData.EquipmentName, _equipmentData.EquipmentDescription);
                    InGameScreenUI.Instance._equipmentDescriptionUI.DisplayOn();

                    if (this._isDestroyable) Destroy(this.gameObject, 2);
                }
            }
        }

        void StopVFX()
        {
            _lootVFX.Stop();
        }

        public bool EquipUserInterface() { return true; }
    }

}
