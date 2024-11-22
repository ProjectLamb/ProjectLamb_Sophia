using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Sophia.DB;
using UnityEngine;

namespace Sophia.Instantiates
{
    using System.Diagnostics;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.Entitys;
    using Sophia.Instantiates.Skills;
    using Sophia.UserInterface;
    using UnityEngine.Events;
    using UnityEngine.VFX;

    public class SkillItemObject : ItemObject
    {
        [SerializeField] public E_SKILL_INDEX _index;
        [SerializeField] public SerialUserInterfaceData _userInterfaceData;
        [SerializeField] public SerialAffectorData _affectorData;
        [SerializeField] public SerialOnDamageExtrasModifierDatas _damageModifierData;
        [SerializeField] public SerialOnConveyAffectExtrasModifierDatas _conveyAffectModifierData;
        [SerializeField] public SerialProjectileInstantiateData _projectileInstantiateData;
        [SerializeField] public SerialAudioData _activatedAudioData;
        [SerializeField] PurchaseComponent _purchaseComponent;
        
        [ContextMenu("Deep Copy To Scriptable")]
        public void Deep_Copy_To_Scriptable(ScriptableSkillData _scriptableSkillData)
        {
            _scriptableSkillData.SetSerial(
                _index,
                _userInterfaceData,
                _affectorData,
                _damageModifierData,
                _conveyAffectModifierData,
                _projectileInstantiateData,
                _activatedAudioData);
        }

        public Skill skill { get; private set; }
        public bool ISDEBUG = true;

        private void Start()
        {
            if (ISDEBUG) { DEBUG_Activate(); }
        }

        protected override void OnTriggerLogic(Collider entity)
        {
            if (!IsReadyToTrigger) return;

            if (entity.TryGetComponent<Entitys.Player>(out Entitys.Player player))
            {
                if (TryGetComponent<PurchaseComponent>(out _purchaseComponent))
                {
                    if (!_purchaseComponent.Purchase(player)) return;
                }
                skill ??= FactoryConcreteSkill.GetSkillByID(_index, player,
                    in _userInterfaceData,
                    in _affectorData,
                    in _damageModifierData,
                    in _conveyAffectModifierData,
                    in _projectileInstantiateData,
                    in _activatedAudioData
                );

                CollectUserInterfaceAction(skill, (bool selected, KeyCode key) =>
                {
                    if (selected)
                    {
                        player.CollectSkill(skill, key);
                        _lootVFX.Stop();
                        _lootObject.SetActive(false);
                        IsReadyToTrigger = false;
                        if (_isDestroyable)
                        {
                            Destroy(gameObject, 2);
                        }
                    }
                });
            }
        }
        public void CollectUserInterfaceAction(Skill skill, UnityAction<bool, KeyCode> action)
        {
            InGameSkillSelector.Instance.OpenSkillSelector(skill, action);
        }
        void StopVFX()
        {
            _lootVFX.Stop();
        }
    }
}
