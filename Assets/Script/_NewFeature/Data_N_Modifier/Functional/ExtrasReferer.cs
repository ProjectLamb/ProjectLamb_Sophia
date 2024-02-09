using UnityEngine;

namespace Sophia.DataSystem.Referer
{
    using Sophia.Composite;
    using Sophia.Entitys;

    public class EntityExtrasReferer
    {
#region Life

            private Extras<DamageInfo> OnDamaged = null;
            private Extras<object> OnDead = null;

#endregion

#region Move

            private Extras<Vector3> OnMove = null;
            private Extras<object> OnIdle = null;

#endregion

#region Affect

            private Extras<Entity> OnOwnerAffected = null;
            private Extras<Entity> OnTargetAffected = null;

#endregion

#region Other

            private Extras<Vector3> OnPhyiscTriggered = null;

#endregion

#region Instantiator

            private Extras<object> OnAttack = null;
            private Extras<object> OnCreated = null;
            private Extras<object> OnTriggerd = null;
            private Extras<object> OnReleased = null;
            private Extras<object> OnForwarding = null;

#endregion

            public virtual void SetRefExtras<T>(Extras<T> extrasRef) {
                switch(extrasRef.FunctionalType) 
                {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {this.OnDamaged = extrasRef as Extras<DamageInfo>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {this.OnDead = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {this.OnMove = extrasRef as Extras<Vector3>;            return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {this.OnIdle = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected :       {this.OnOwnerAffected = extrasRef as Extras<Entity>;    return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected :      {this.OnTargetAffected = extrasRef as Extras<Entity>;   return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {this.OnPhyiscTriggered = extrasRef as Extras<Vector3>; return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {this.OnAttack = extrasRef as Extras<object>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {this.OnCreated = extrasRef as Extras<object>;          return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {this.OnTriggerd = extrasRef as Extras<object>;         return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {this.OnReleased = extrasRef as Extras<object>;         return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {this.OnForwarding = extrasRef as Extras<object>;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.None : {
                        throw new System.Exception($"참조 Extras가 초기화되지 않음");
                    }
                    default : {
                        throw new System.Exception($"이 Entity 멤버에는 {extrasRef.FunctionalType.ToString()} 없음");
                    }
                }
            }
            public virtual Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<T> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {res = this.OnDamaged as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected :            {res = this.OnOwnerAffected as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected :            {res = this.OnTargetAffected as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {res = this.OnPhyiscTriggered as Extras<T>;     break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack as Extras<T>;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {res = this.OnReleased as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {res = this.OnForwarding as Extras<T>;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.None :                
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public virtual Extras<DamageInfo> GetExtrasDamageInfo(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<DamageInfo> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :         {res = this.OnDamaged; break;   }
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }

            public virtual Extras<object> GetExtrasNull(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<object> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack;        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated;       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {res = this.OnReleased;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {res = this.OnForwarding;    break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public virtual Extras<Entity> GetExtrasEntity(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<Entity> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected : {res = this.OnOwnerAffected; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected : {res = this.OnTargetAffected; break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public virtual Extras<Vector3> GetExtrasVector(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<Vector3> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {res = this.OnPhyiscTriggered; break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
    }

    public class PlayerExtrasReferer : EntityExtrasReferer{
#region Life

            private Extras<DamageInfo> OnDamaged = null;
            private Extras<object> OnDead = null;

#endregion

#region Move

            private Extras<Vector3> OnMove = null;
            private Extras<object> OnIdle = null;

#endregion

#region Affect

            private Extras<Entity> OnOwnerAffected = null;
            private Extras<Entity> OnTargetAffected = null;

#endregion

#region Other

            private Extras<Vector3> OnPhyiscTriggered = null;

#endregion

#region Instantiator

            private Extras<object> OnAttack = null;
            private Extras<object> OnCreated = null;
            private Extras<object> OnTriggerd = null;
            private Extras<object> OnReleased = null;
            private Extras<object> OnForwarding = null;
#endregion

#region Player
        
            private Extras<object> OnDash = null;
            private Extras<object> OnSkill = null;
            private Extras<object> OnWeaponUse = null;
            private Extras<object> OnProjectileRestore = null;
            private Extras<object> OnSkillUse = null;
            private Extras<object> OnSkillRefilled = null;
            
#endregion

#region Equipment
            

#endregion

#region Global
#endregion
            public override void SetRefExtras<T>(Extras<T> extrasRef) {
                switch(extrasRef.FunctionalType) 
                {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {this.OnDamaged = extrasRef as Extras<DamageInfo>;               return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {this.OnDead = extrasRef as Extras<object>;                 return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {this.OnMove = extrasRef as Extras<Vector3>;                return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {this.OnIdle = extrasRef as Extras<object>;                 return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected :       {this.OnOwnerAffected = extrasRef as Extras<Entity>;        return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected :      {this.OnTargetAffected = extrasRef as Extras<Entity>;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {this.OnPhyiscTriggered = extrasRef as Extras<Vector3>;     return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {this.OnAttack = extrasRef as Extras<object>;               return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {this.OnCreated = extrasRef as Extras<object>;              return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {this.OnTriggerd = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {this.OnReleased = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {this.OnForwarding = extrasRef as Extras<object>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :                {this.OnDash = extrasRef as Extras<object> ;                return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.skill :               {this.OnSkill = extrasRef as Extras<object> ;               return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse :           {this.OnWeaponUse = extrasRef as Extras<object> ;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :   {this.OnProjectileRestore = extrasRef as Extras<object>;    return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse :            {this.OnSkillUse = extrasRef as Extras<object> ;            return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :       {this.OnSkillRefilled = extrasRef as Extras<object> ;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.None : {
                        throw new System.Exception($"참조 Extras가 초기화되지 않음");
                    }
                    default : {
                        throw new System.Exception($"이 Entity 멤버에는 {extrasRef.FunctionalType.ToString()} 없음");
                    }
                }
            }

            public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<T> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {res = this.OnDamaged as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected :       {res = this.OnOwnerAffected as Extras<T>;       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected :      {res = this.OnTargetAffected as Extras<T>;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {res = this.OnPhyiscTriggered as Extras<T>;     break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack as Extras<T>;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {res = this.OnReleased as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {res = this.OnForwarding as Extras<T>;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :                {res = this.OnDash as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.skill :               {res = this.OnSkill as Extras<T>;               break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse :           {res = this.OnWeaponUse as Extras<T>;           break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :   {res = this.OnProjectileRestore as Extras<T>;   break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse :            {res = this.OnSkillUse as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :       {res = this.OnSkillRefilled as Extras<T>;       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.None :                
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public override Extras<DamageInfo> GetExtrasDamageInfo(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<DamageInfo> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :         {res = this.OnDamaged; break;   }
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }

            public override Extras<object> GetExtrasNull(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<object> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle;          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack;        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated;       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {res = this.OnReleased;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {res = this.OnForwarding;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :          {res = this.OnDash;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.skill :          {res = this.OnSkill;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse :          {res = this.OnWeaponUse;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :          {res = this.OnProjectileRestore;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse :          {res = this.OnSkillUse;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :          {res = this.OnSkillRefilled;    break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public override Extras<Entity> GetExtrasEntity(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<Entity> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.OwnerAffected : {res = this.OnOwnerAffected; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.TargetAffected : {res = this.OnTargetAffected; break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
            public override Extras<Vector3> GetExtrasVector(E_FUNCTIONAL_EXTRAS_TYPE functionalType) {
                Extras<Vector3> res = null;
                switch(functionalType) {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhyiscTriggered :     {res = this.OnPhyiscTriggered; break;}
                    default: {
                        throw new System.Exception($"이 Entity 멤버에는 {functionalType.ToString()} 없음");
                    }
                }
                if(res == null) {
                    throw new System.Exception($"참조하려는 {functionalType.ToString()} 멤버가 초기화되지 않음");
                }
                return res;
            }
    }
}