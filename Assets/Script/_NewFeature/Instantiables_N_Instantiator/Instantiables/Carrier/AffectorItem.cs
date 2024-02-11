
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
                    player.Affect(AffectFactory(_affectData));
                    //Destroy(this.gameObject);
                }
            }
        }
        public bool EquipUserInterface() { return true; }

        public Affector AffectFactory(SerialAffectorData affectData) {
            switch(affectData._affectType) 
            {
                case E_AFFECT_TYPE.Burn         :   {return new BurnAffect(_affectData);}
                case E_AFFECT_TYPE.Poisoned     :   {return new PoisonedAffect(_affectData);}
                case E_AFFECT_TYPE.Bleed        :   {return new BleedAffect(_affectData);}
                case E_AFFECT_TYPE.Cold         :   {return new ColdAffect(_affectData);}
                case E_AFFECT_TYPE.Stern        :   {return new SternAffect(_affectData);}
                case E_AFFECT_TYPE.Bounded      :   {return new BoundedAffect(_affectData);}
                case E_AFFECT_TYPE.Knockback    :   {return new KnockbackAffect(_affectData);}
                case E_AFFECT_TYPE.BlackHole    :   {return new BlackHoleAffect(_affectData);}
                case E_AFFECT_TYPE.Airborne    :    {return new AirborneAffect(_affectData);}
                default : {return null;}
            }
        }
    }
}