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
    using Sophia.Instantiates.Skills;
    using Sophia.UserInterface;
    using UnityEngine.Events;
    using UnityEngine.VFX;

    public class SkillItem : Carrier
    {
        [SerializeField] public E_SKILL_INDEX _index;
        [SerializeField] public SerialUserInterfaceData     _userInterfaceData;
        [SerializeField] public SerialAffectorData          _affectorData;
        [SerializeField] public SerialOnDamageExtrasModifierDatas _damageModifierData;
        [SerializeField] public SerialOnConveyAffectExtrasModifierDatas _conveyAffectModifierData; 
        [SerializeField] public SerialProjectileInstantiateData _projectileInstantiateData; 

        public Skill skill  { get; private set; }

        [SerializeField] public GameObject lootObject;
        [SerializeField] public VisualEffect lootVFX;
        public bool triggeredOnce = false;
        
        protected void Start()
        {
            lootVFX.Play();
        }
        
        protected override void OnTriggerLogic(Collider entity)
        {
            if(triggeredOnce) return;
            if(entity.TryGetComponent<Entitys.Player>(out Entitys.Player player)) {
                skill ??= FactoryConcreteSkill.GetSkillByID(_index, player,
                    in _userInterfaceData,
                    in _affectorData,
                    in _damageModifierData,
                    in _conveyAffectModifierData,
                    in _projectileInstantiateData
                );
                CollectUserInterfaceAction(skill, (bool selected, KeyCode key) => {
                    if(selected) {
                        skill.AddToUpdator();
                        player.CollectSkill(skill, key);
                        lootVFX.Stop();
                        lootObject.SetActive(false);
                        triggeredOnce = true;
                        if(this._isDestroyable){
                            Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                            Destroy (gameObject, 3);
                        }
                    }
                });
            }
        }
        public void CollectUserInterfaceAction(Skill skill, UnityAction<bool, KeyCode> action) { 
            InGameSkillSelector.Instance.OpenSkillSelector(skill, action);
        }
    }
}