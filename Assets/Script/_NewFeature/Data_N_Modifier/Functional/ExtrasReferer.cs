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
            private Extras<int>    OnHealthTriggered = null;

#endregion

#region Move

            private Extras<Vector3> OnMove = null;
            private Extras<object> OnIdle = null;

#endregion

#region Affect

            private Extras<Entity> OnConveyAffect = null;

#endregion

#region Other

            private Extras<Vector3> OnPhysicTriggered = null;

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
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {this.OnMove = extrasRef as Extras<Vector3>;            return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {this.OnDamaged = extrasRef as Extras<DamageInfo>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {this.OnAttack = extrasRef as Extras<object>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :        {this.OnConveyAffect = extrasRef as Extras<Entity>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {this.OnDead = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {this.OnIdle = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {this.OnPhysicTriggered = extrasRef as Extras<Vector3>; return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {this.OnCreated = extrasRef as Extras<object>;          return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {this.OnTriggerd = extrasRef as Extras<object>;         return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.HealthTriggered :     {this.OnHealthTriggered = extrasRef as Extras<int>;         return;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {res = this.OnDamaged as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack as Extras<T>;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :        {res = this.OnConveyAffect as Extras<T>;        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle as Extras<T>;                break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {res = this.OnPhysicTriggered as Extras<T>;     break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated as Extras<T>;             break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd as Extras<T>;            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.HealthTriggered :     {res = this.OnHealthTriggered as Extras<T>;            break;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :        {res = this.OnConveyAffect; break;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {res = this.OnPhysicTriggered; break;}
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

            private Extras<DamageInfo>          OnDamaged = null;
            private Extras<object>              OnDead = null;

#endregion

#region Move

            private Extras<Vector3>             OnMove = null;
            private Extras<object>              OnIdle = null;

#endregion

#region Affect
            
            private Extras<Entity>              OnConveyAffect = null;
            private Extras<Entity>              OnWeaponConveyAffect = null;
            private Extras<Entity>              OnSkillConveyAffect = null;

#endregion

#region CarrierObject Trigger

            private Extras<Vector3>             OnPhysicTriggered = null;
            private Extras<int>                 OnGearcoinTriggered = null;
            private Extras<int>                 OnHealthTriggered = null;

#endregion

#region Instantiator

            private Extras<object>              OnAttack = null;
            private Extras<object>              OnCreated = null;
            private Extras<object>              OnTriggerd = null;
            private Extras<object>              OnReleased = null;
            private Extras<object>              OnForwarding = null;

#endregion

#region Player
        
            private Extras<object>              OnDash = null;
            private Extras<object>              OnSkill = null;
            private Extras<DamageInfo>          OnWeaponUse = null;
            private Extras<object>              OnProjectileRestore = null;
            private Extras<DamageInfo>          OnSkillUse = null;
            private Extras<object>              OnSkillRefilled = null;
            
#endregion

#region Global
        
            private Extras<object>               OnEnemyHit = null;
            private Extras<object>               OnEnemyDie = null;    
            private Extras<Stage>                OnStageClear = null;
            private Extras<(Stage, Stage)>       OnStageEnter = null;

#endregion
            public override void SetRefExtras<T>(Extras<T> extrasRef) {
                switch(extrasRef.FunctionalType) 
                {
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {this.OnMove = extrasRef as Extras<Vector3>;                return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {this.OnDamaged = extrasRef as Extras<DamageInfo>;          return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {this.OnAttack = extrasRef as Extras<object>;               return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :        {this.OnConveyAffect = extrasRef as Extras<Entity>;         return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect :  {this.OnWeaponConveyAffect = extrasRef as Extras<Entity>;   return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillConveyAffect :   {this.OnSkillConveyAffect = extrasRef as Extras<Entity>;    return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {this.OnDead = extrasRef as Extras<object>;                 return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {this.OnIdle = extrasRef as Extras<object>;                 return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {this.OnPhysicTriggered = extrasRef as Extras<Vector3>;     return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {this.OnCreated = extrasRef as Extras<object>;              return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {this.OnTriggerd = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {this.OnReleased = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {this.OnForwarding = extrasRef as Extras<object>;           return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :                {this.OnDash = extrasRef as Extras<object> ;                return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Skill :               {this.OnSkill = extrasRef as Extras<object> ;               return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse :           {this.OnWeaponUse = extrasRef as Extras<DamageInfo> ;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :   {this.OnProjectileRestore = extrasRef as Extras<object>;    return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse :            {this.OnSkillUse = extrasRef as Extras<DamageInfo> ;        return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :       {this.OnSkillRefilled = extrasRef as Extras<object> ;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered :   {this.OnGearcoinTriggered = extrasRef as Extras<int>;       return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.HealthTriggered :     {this.OnHealthTriggered = extrasRef as Extras<int>;         return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyHit :            {this.OnEnemyHit = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie :            {this.OnEnemyDie = extrasRef as Extras<object>;             return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.StageClear :          {this.OnStageClear = extrasRef as Extras<Stage>;            return;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.StageEnter :          {this.OnStageEnter = extrasRef as Extras<(Stage, Stage)>;   return;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.Move :                {res = this.OnMove as Extras<T>;                            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged :             {res = this.OnDamaged as Extras<T>;                         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              {res = this.OnAttack as Extras<T>;                          break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :        {res = this.OnConveyAffect as Extras<T>;                    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect :  {res = this.OnWeaponConveyAffect as Extras<T>;              break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillConveyAffect :   {res = OnSkillConveyAffect as Extras<T>;                    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                {res = this.OnDead as Extras<T>;                            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                {res = this.OnIdle as Extras<T>;                            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {res = this.OnPhysicTriggered as Extras<T>;                 break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             {res = this.OnCreated as Extras<T>;                         break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            {res = this.OnTriggerd as Extras<T>;                        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            {res = this.OnReleased as Extras<T>;                        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          {res = this.OnForwarding as Extras<T>;                      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :                {res = this.OnDash as Extras<T>;                            break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.Skill :               {res = this.OnSkill as Extras<T>;                           break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse :           {res = this.OnWeaponUse as Extras<T>;                       break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :   {res = this.OnProjectileRestore as Extras<T>;               break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse :            {res = this.OnSkillUse as Extras<T>;                        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :       {res = this.OnSkillRefilled as Extras<T>;                   break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.GearcoinTriggered :   {res = this.OnGearcoinTriggered as Extras<T>;               break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.HealthTriggered :     {res = this.OnHealthTriggered as Extras<T>;                 break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyHit :            {res = this.OnEnemyHit as Extras<T>;                        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie :            {res = this.OnEnemyDie as Extras<T>;                        break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.StageClear :          {res = this.OnStageClear as Extras<T>;                      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.StageEnter :          {res = this.OnStageEnter as Extras<T>;                      break;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.Damaged       :   {res = this.OnDamaged;      break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponUse     :   {res = this.OnWeaponUse;    break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillUse      :   {res = this.OnSkillUse;     break;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dead :                { res = this.OnDead;                 break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Idle :                { res = this.OnIdle;                 break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Attack :              { res = this.OnAttack;               break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Created :             { res = this.OnCreated;              break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Triggerd :            { res = this.OnTriggerd;             break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Released :            { res = this.OnReleased;             break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Forwarding :          { res = this.OnForwarding;           break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Dash :                { res = this.OnDash;                 break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.Skill :               { res = this.OnSkill;                break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.ProjectileRestore :   { res = this.OnProjectileRestore;    break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillRefilled :       { res = this.OnSkillRefilled;        break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyHit :            { res = this.OnEnemyHit;             break; }
                    case E_FUNCTIONAL_EXTRAS_TYPE.EnemyDie :            { res = this.OnEnemyDie;             break; }
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect :    {res = this.OnConveyAffect; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.WeaponConveyAffect :    {res = this.OnWeaponConveyAffect; break;}
                    case E_FUNCTIONAL_EXTRAS_TYPE.SkillConveyAffect :    {res = OnSkillConveyAffect; break;}
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
                    case E_FUNCTIONAL_EXTRAS_TYPE.PhysicTriggered :     {res = this.OnPhysicTriggered; break;}
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