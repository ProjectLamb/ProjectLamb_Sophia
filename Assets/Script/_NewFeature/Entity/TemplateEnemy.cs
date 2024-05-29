using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sophia.Composite;
using Sophia.DataSystem;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem.Referer;
using UnityEngine;

namespace Sophia.Entitys
{
    public class TemplateEnemy : Entitys.Enemy, IMovable
    {
#region __EntityParents__
    /*********************************************************************************
    
    SerializeMember 
        
        [SerializeField] protected ModelManager          _modelManager;
        [SerializeField] protected EntityAudioManager   _audioManager;
        [SerializeField] protected VisualFXBucket       _visualFXBucket;

    Members

        [HideInInspector] public Collider entityCollider;
        [HideInInspector] public Rigidbody entityRigidbody;
        [HideInInspector] protected List<IDataSettable> Settables = new();

    *********************************************************************************/

    #region Initialize
        protected override void CollectSettable()
        {
            Settables.Add(Life);
            Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        protected override void SetDataToReferer()
        {
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }
        
    #endregion

    #region Life Accessible

        private LifeComposite Life { get; set; }
        public override LifeComposite GetLifeComposite() => Life;
        public override bool GetDamaged(DamageInfo damage) 
        {
            bool isDamaged = false;
            if (Life.IsDie) { isDamaged = false; }
            else
            {
                if (isDamaged = Life.Damaged(damage))
                {
                    GameManager.Instance.NewFeatureGlobalEvent.OnEnemyHitEvent.Invoke();
                }
            }
            if (Life.IsDie) { Die(); }
            return isDamaged;
        }
        public override bool Die() 
{
            Life.Died(); return true;
        }
    #endregion

    #region Data Accessible
        public override EntityStatReferer GetStatReferer() 
        {
            throw new System.NotImplementedException();
        }
        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) 
        {
            throw new System.NotImplementedException();
        }
        public override string GetStatsInfo() 
        {
            throw new System.NotImplementedException();
        }
        public override EntityExtrasReferer GetExtrasReferer() 
        {
            throw new System.NotImplementedException();
        }
        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) 
        {
            throw new System.NotImplementedException();
        }
    #endregion

    #region Affector Accessible
    
        public override AffectorManager GetAffectorManager()    => this._affectorManager ??= GetComponentInChildren<AffectorManager>();
        public override void Affect(Affector affector)          => this._affectorManager.Affect(affector);
        public override void Recover(Affector affector)         => this._affectorManager.Recover(affector);
        
    #endregion

#endregion

#region __EnemyParents__

    /*********************************************************************************
        
        [Header("Mob Settings")]
        [SerializeField] protected SerialBaseEntityData       _baseEntityData;
        [SerializeField] protected SerialFieldOfViewData      _fOVData;
        [SerializeField] protected AffectorManager            _affectorManager;
        [SerializeField] protected ProjectileBucketManager    _projectileBucketManager;
        [SerializeField] protected ProjectileObject[]         _attckProjectiles;
        [SerializeField] protected VisualFXObject             _spawnParticleRef;
        [SerializeField] protected VisualFXObject             _dieParticleRef;
        [SerializeField] protected E_MOB_AI_DIFFICULTY        _mobDifficulty;
        [SerializeField] public    Entity                     _objectiveEntity;

    *********************************************************************************/

    #region AI
        public override RecognizeEntityComposite GetRecognizeComposite()
        {
            throw new System.NotImplementedException();    
        }
    #endregion
#endregion

#region Move

        private bool IsMovable = false;
        public bool GetMoveState() => IsMovable;
        public void SetMoveState(bool movableState) => IsMovable = movableState;
        public void MoveTick() {
            if(!IsMovable) return;
        }
        public UniTask Turning(Vector3 forwardingVector) {
            throw new System.NotImplementedException();
        }

#endregion

        protected override void Awake()
        {
            base.Awake();

            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            _affectorManager.Init(_baseEntityData.Tenacity);
        }

        protected override void Start()
        {
            base.Start();

            // Life.OnEnterDie += OnEnterDie;
            // Life.OnExitDie += OnExitDie;

        }
    }
}