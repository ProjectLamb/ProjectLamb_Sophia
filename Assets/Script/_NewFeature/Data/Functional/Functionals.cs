using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph;
using UnityEngine.Events;
using System;

namespace Sophia.DataSystem.Functional
{
    using Sophia.Entitys;

    public class EntityExtrasReferer
    {
#region Life

            public Extras<float> OnHit {get; set;}
            public Extras<float> OnDamaged {get; set;}
            public Extras<object> OnDead {get; set;}

#endregion

#region Move

            public Extras<Vector3> OnMove {get; set;}
            public Extras<object> OnIdle {get; set;}

#endregion

#region Affect

            public Extras<Entity> OnAffected {get; set;}

#endregion

#region Other

            public Extras<Vector3> OnPhyiscTriggered {get; set;}

#endregion

#region Instantiator

            public Extras<object> OnAttack {get; set;}
            public Extras<object> OnCreated {get; set;}
            public Extras<object> OnTriggerd {get; set;}
            public Extras<object> OnReleased {get; set;}
            public Extras<object> OnForwarding {get; set;}

#endregion
    }

    public class PlayerExtrasReferer : EntityExtrasReferer{
#region Life

//          public Extras<float> OnHit {get; set;}
//          public Extras<float> OnDamaged {get; set;}
//          public Extras<object> OnDead {get; set;}

#endregion

#region Move

//          public Extras<Vector3> OnMove {get; set;}
//          public Extras<object> OnIdle {get; set;}

#endregion

#region Affect

//          public Extras<Entity> OnAffected {get; set;}

#endregion

#region Other

//          public Extras<Vector3> OnPhyiscTriggered {get; set;}

#endregion

#region Instantiator

//          public Extras<object> OnAttack {get; set;}
//          public Extras<object> OnCreated {get; set;}
//          public Extras<object> OnTriggerd {get; set;}
//          public Extras<object> OnReleased {get; set;}
//          public Extras<object> OnForwarding {get; set;}
#endregion

#region Player
        
            public Extras<object> OnDash {get; set;}
            public Extras<object> Onskill {get; set;}
            public Extras<object> OnWeaponUse {get; set;}
            public Extras<object> OnProjectileRestore {get; set;}
            public Extras<object> OnSkillUse {get; set;}
            public Extras<object> OnSkillRefilled {get; set;}
            
#endregion
    }
}