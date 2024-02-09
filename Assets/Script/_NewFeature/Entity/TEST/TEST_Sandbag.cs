using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sophia.Entitys
{
    using Cysharp.Threading.Tasks;
    using Sophia.Composite;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Referer;
    using Sophia.Instantiates;
    public class TEST_Sandbag : Entity, IAttackable 
    {

#region SerializeMember
        [SerializeField] private SerialBaseEntityData _baseEntityData;
        [SerializeField] private AffectorManager _affectorManager;
        [SerializeField] private ProjectileBucket       _projectileBucket;

#endregion

#region Members
        public LifeComposite Life {get; private set;}
        public EntityStatReferer StatReferer { get; private set; }
        public EntityExtrasReferer ExtrasReferer { get; private set; }
#endregion

#region Life Accessible

        public override LifeComposite GetLifeComposite() => Life;
        public override bool GetDamaged(DamageInfo damage) {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else {isDamaged = Life.Damaged(damage);}
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }
        public override bool Die() {
            Destroy(gameObject, 0.5f);
            return true;
        }

#endregion

#region Stat Accessible 

        public override EntityStatReferer GetStatReferer() => StatReferer;
        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);
        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);
        [ContextMenu("Get Stats Info")]
        public override string GetStatsInfo() {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

#endregion

#region Affectable

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);

#endregion

#region Attackable
        public async void Attack()
        {
            try
            {
                await Turning();
                _weaponManager.GetCurrentWeapon().Use(this);
            }
            catch (OperationCanceledException)
            {

            }
        }
#endregion

#region Movable
        public MovementComposite GetMovementComposite()
        {
            throw new System.NotImplementedException();
        }

        public bool GetMoveState()
        {
            throw new System.NotImplementedException();
        }

        public void SetMoveState(bool movableState)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTick()
        {
            throw new System.NotImplementedException();
        }

        public UniTask Turning()
        {
            throw new System.NotImplementedException();
        }


    }
}