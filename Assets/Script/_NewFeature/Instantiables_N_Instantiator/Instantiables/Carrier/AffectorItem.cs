
using UnityEngine;

namespace Sophia.Instantiates 
{
    
    using Sophia.Entitys;
    using Sophia.DataSystem.Modifiers.ConcreteAffector;
    using FMOD;
    using Sophia.DataSystem.Modifiers;

    public class AffectorItem : Carrier
    {
        [SerializeField] SerialAffectorData _affectData;
        
        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent(out Player player))
            {
                if(EquipUserInterface()){
                    player.Affect(FactoryConcreteAffect.GetAffectByID(in _affectData, player));
                }
                Destroy(this.gameObject);
            }
        }
        public bool EquipUserInterface() { return true; }
    }
}