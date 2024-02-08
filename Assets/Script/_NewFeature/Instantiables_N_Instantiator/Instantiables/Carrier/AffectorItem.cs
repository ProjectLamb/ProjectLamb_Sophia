
using UnityEngine;

namespace Sophia.Instantiates 
{
    
    using Sophia.Entitys;
    using Sophia.DataSystem.Modifiers.NewConcreteAffector;
    

    public class AffectorItem : Carrier
    {
        [SerializeField] SerialAffectorData _affectData;
        
        public PoisonedAffect poisonedAffect;

        private void Awake() {
            poisonedAffect = new PoisonedAffect(_affectData);
        }
        
        protected override void OnTriggerLogic(Collider entity)
        {
            if(entity.TryGetComponent(out Player player))
            {
                if(EquipUserInterface()){
                    player.Affect(poisonedAffect);
                    Destroy(this.gameObject);
                }
            }
        }

        public bool EquipUserInterface() { return true; }
    }
}