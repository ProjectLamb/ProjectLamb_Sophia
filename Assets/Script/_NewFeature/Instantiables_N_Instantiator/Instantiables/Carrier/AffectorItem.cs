
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
                    player.Affect(AffectFactory(in _affectData, player));
                    //Destroy(this.gameObject);
                }
            }
        }
        public bool EquipUserInterface() { return true; }

        public Affector AffectFactory(in SerialAffectorData affectData, Entitys.Entity entity) {
            switch(affectData._affectType) 
            {
                case E_AFFECT_TYPE.Burn         :   {return new BurnAffect(in _affectData);}
                case E_AFFECT_TYPE.Poisoned     :   {return new PoisonedAffect(in _affectData);}
                case E_AFFECT_TYPE.Bleed        :   {return new BleedAffect(in _affectData);}
                case E_AFFECT_TYPE.Cold         :   {return new ColdAffect(in _affectData);}
                case E_AFFECT_TYPE.Stern        :   {return new SternAffect(in _affectData);}
                case E_AFFECT_TYPE.Bounded      :   {return new BoundedAffect(in _affectData);}
                case E_AFFECT_TYPE.Knockback    :   {return new KnockbackAffect(in _affectData, entity.GetGameObject().transform);}
                case E_AFFECT_TYPE.BlackHole    :   {return new BlackHoleAffect(in _affectData, entity.GetGameObject().transform);}
                case E_AFFECT_TYPE.Airborne    :    {return new AirborneAffect(in _affectData);}
                default : {return null;}
            }
        }
    }
}