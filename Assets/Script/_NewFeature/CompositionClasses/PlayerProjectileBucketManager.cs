using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite;
    using System.Linq;

    public class PlayerProjectileBucketManager : ProjectileBucketManager {
        public Extras<Entity> WeaponConveyAffectExtras    {get; protected set;}
        public Extras<Entity> SkillConveyAffectExtras    {get; protected set;}
        
        public override void Init(in SerialBaseInstantiatorData baseInstantiatorData) {
            base.Init(in baseInstantiatorData);

            WeaponConveyAffectExtras  = new Extras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect, OnConveyAffectUpdated);
            SkillConveyAffectExtras  = new Extras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.SkillConveyAffect, OnConveyAffectUpdated);
        }
        public override void SetStatDataToReferer(EntityStatReferer statReferer) {
            base.SetStatDataToReferer(statReferer);
        }
        public override void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer) {
            base.SetExtrasDataToReferer(extrasReferer);

            extrasReferer.SetRefExtras<Entity>(WeaponConveyAffectExtras);
            extrasReferer.SetRefExtras<Entity>(SkillConveyAffectExtras);
        }
    }
}