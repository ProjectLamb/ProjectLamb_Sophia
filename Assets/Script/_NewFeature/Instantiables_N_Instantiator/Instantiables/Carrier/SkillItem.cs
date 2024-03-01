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
    using UnityEngine.VFX;

    public class SkillItem : Carrier
    {
        [SerializeField] private GameObject lootObject;
        [SerializeField] private VisualEffect lootVFX;
        [SerializeField] private E_SKILL_INDEX _index;
        [SerializeField] private SerialUserInterfaceData     _userInterfaceData;
        [SerializeField] private SerialAffectorData          _affectorData;
        [SerializeField] private SerialOnDamageExtrasModifierDatas _damageModifierData;
        [SerializeField] private SerialOnConveyAffectExtrasModifierDatas _executeModifierData; 
        [SerializeField] private SerialProjectileInstantiateData _projectileInstantiateData; 

        public Skill skillInstance; 
        
        protected void Start()
        {
            lootVFX.Play();
        }
        
        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent<Entitys.Player>(out Entitys.Player player)) {
                skillInstance = FactoryConcreteSkill.GetSkillByID(_index, player,
                    in _userInterfaceData,
                    in _affectorData,
                    in _damageModifierData,
                    in _executeModifierData,
                    in _projectileInstantiateData
                );
                skillInstance.AddToUpdator();
                player.CollectSkill(skillInstance, KeyCode.Q);
                
                lootVFX.Stop();
                lootObject.SetActive(false);
                Destroy (gameObject, 5);
            }
        }
        public bool EquipUserInterface() { return true; }
    }
}