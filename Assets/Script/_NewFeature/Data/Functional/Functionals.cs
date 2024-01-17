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
            
            /***
            이거.. 내가 하면서도 제대로 하는건지 모르겠네
            제네릭 타입을 런타임에 정해줘서 반환이 가능하다는건가?? 
            이게 가능하다고? 흠..
            */
            
            public Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType){
                Extras<T> res = null;
                switch(functionalType) 
                {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move:             {res = OnMove;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Hit:              {res = OnHit;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged:          {res = OnDamaged;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack:           {res = OnAttack;           break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Affected:         {res = OnAffected;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead:             {res = OnDead;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Standing:         {res = OnIdle;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered:  {res = OnPhyiscTriggered;  break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created:          {res = OnCreated;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd:         {res = OnTriggerd;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released:         {res = OnReleased;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding:       {res = OnForwarding;       break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                
                if(res == null) { throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음"); }
                return res;
            }
    }

    public class PlayerExtrasReferer {
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

#region Player
        
            public Extras<object> OnDash {get; set;}
            public Extras<object> Onskill {get; set;}
            public Extras<object> OnWeaponUse {get; set;}
            public Extras<object> OnProjectileRestore {get; set;}
            public Extras<object> OnSkillUse {get; set;}
            public Extras<object> OnSkillRefilled {get; set;}
            
#endregion
            
            /***
            이거.. 내가 하면서도 제대로 하는건지 모르겠네
            제네릭 타입을 런타임에 정해줘서 반환이 가능하다는건가?? 
            이게 가능하다고? 흠..
            */
            
            public Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType){
                Extras<T> res = null;
                switch(functionalType) 
                {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move:             {res = OnMove;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Hit:              {res = OnHit;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged:          {res = OnDamaged;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack:           {res = OnAttack;           break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Affected:         {res = OnAffected;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead:             {res = OnDead;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Standing:         {res = OnIdle;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered:  {res = OnPhyiscTriggered;  break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created:          {res = OnCreated;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd:         {res = OnTriggerd;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released:         {res = OnReleased;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding:       {res = OnForwarding;       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash:       {res = OnDash;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.skill:       {res = Onskill;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse:       {res = OnWeaponUse;        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore:       {res = OnProjectileRestore;break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse:       {res = OnSkillUse;         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled:       {res = OnSkillRefilled;    break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                
                if(res == null) { throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음"); }
                return res;
            }
    }
}