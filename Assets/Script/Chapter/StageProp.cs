using System.Collections;
using System.Collections.Generic;
using Sophia.Composite;
using Sophia.DataSystem;
using Sophia.DataSystem.Modifiers;
using Sophia.DataSystem.Referer;
using UnityEngine;
using DG.Tweening;
using FMODPlus;

namespace Sophia.Entitys
{
    using Sophia.Instantiates;

    public enum E_STAGEPROP_AUDIO_INDEX
    {
        Hit, Death
    }

    public class StageProp : Entity
    {
        #region Serialized Member
        [SerializeField] protected AffectorManager _affectorManager;
        [SerializeField] protected SerialBaseEntityData _baseEntityData;
        [SerializeField] protected VisualFXObject _dieParticleRef;
        [SerializeField] protected List<FMODAudioSource> _audioSources;

        #endregion

        #region Private
        private LifeComposite Life { get; set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            Life = new LifeComposite(_baseEntityData.MaxHp, _baseEntityData.Defence);
            _affectorManager.Init(_baseEntityData.Tenacity);
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            Life.OnDamaged += OnPropHit;
            Life.OnEnterDie += OnPropEnterDie;
            Life.OnExitDie += OnPropExitDie;
        }

        public void OnPropHit(DamageInfo damageInfo)
        {
            // GetModelManager().GetAnimator().SetTrigger("DoHit");
            //GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Damaged].PlayFunctionalActOneShotWithDuration(0.3f);
            _audioSources[(int)E_STAGEPROP_AUDIO_INDEX.Hit].Play();
            GameManager.Instance.NewFeatureGlobalEvent.EnemyHit.PerformStartFunctionals(ref GlobalHelper.NullRef);
        }

        public void OnPropEnterDie()
        {
            /** VFX **/
            //GetModelManager().GetMaterialVFX().FunctionalMaterialChanger[E_FUNCTIONAL_EXTRAS_TYPE.Dead].PlayFunctionalActOneShotWithDuration(0.5f);
            VisualFXObject visualFX = VisualFXObjectPool.GetObject(_dieParticleRef).Init();
            GetVisualFXBucket().InstantablePositioning(visualFX)?.Activate();

            /** Audio **/
            _audioSources[(int)E_STAGEPROP_AUDIO_INDEX.Death].Play();

            List<Sophia.Instantiates.ItemObject> itemObjects;
            itemObjects = GetComponent<Sophia.Instantiates.GachaComponent>().InstantiateReward();

            foreach (Sophia.Instantiates.ItemObject itemObject in itemObjects)
            {
                if (itemObject == null) continue;
                itemObject.SetTriggerTime(1f).SetTweenSequence(SetSequnce(itemObject)).Activate();
            }

            entityCollider.enabled = false;
        }

        public Sequence SetSequnce(Sophia.Instantiates.ItemObject itemObject)
        {
            Sequence mySequence = DOTween.Sequence();
            System.Random random = new System.Random();
            Vector3 EndPosForward = transform.right;
            var randomAngle = 0;
            Vector3[] rotateMatrix = new Vector3[] {
                new Vector3(Mathf.Cos(randomAngle), 0 , Mathf.Sin(randomAngle)),
                new Vector3(0, 1 , 0),
                new Vector3(-Mathf.Sin(randomAngle), 0 , Mathf.Cos(randomAngle))
            };
            Vector3 retatedVec = Vector3.zero + Vector3.up;
            retatedVec += EndPosForward.x * rotateMatrix[0];
            retatedVec += EndPosForward.y * rotateMatrix[1];
            retatedVec += EndPosForward.z * rotateMatrix[2];
            Tween jumpTween = itemObject.transform.DOLocalJump(retatedVec + transform.position, 10, 1, 1).SetEase(Ease.OutBounce);
            return mySequence.Append(jumpTween);
        }

        public void OnPropExitDie()
        {
            Destroy(gameObject, 0.5f);
        }
        public override bool Die() { Life.Died(); return true; }

        protected override void SetDataToReferer()
        {
            this.Settables.ForEach(E =>
            {
                E.SetStatDataToReferer(StatReferer);
                E.SetExtrasDataToReferer(ExtrasReferer);
            });
        }

        public override EntityStatReferer GetStatReferer() => this.StatReferer;

        protected override void CollectSettable()
        {
            Settables.Add(Life);
            // Settables.Add(_projectileBucketManager);
            Settables.Add(_affectorManager);
        }

        public override LifeComposite GetLifeComposite() => this.Life;

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
            if (Life.IsDie)
            {
                Die();
            }
            return isDamaged;
        }

        public override Stat GetStat(E_NUMERIC_STAT_TYPE numericType) => StatReferer.GetStat(numericType);

        public override string GetStatsInfo()
        {
            Debug.Log(this.StatReferer.GetStatsInfo());
            return this.StatReferer.GetStatsInfo();
        }

        public override EntityExtrasReferer GetExtrasReferer() => ExtrasReferer;

        public override Extras<T> GetExtras<T>(E_FUNCTIONAL_EXTRAS_TYPE functionalType) => ExtrasReferer.GetExtras<T>(functionalType);

        public override AffectorManager GetAffectorManager() => this._affectorManager ??= GetComponentInChildren<AffectorManager>();

        public override void Affect(Affector affector) => this._affectorManager.Affect(affector);

        public override void Recover(Affector affector) => this._affectorManager.Recover(affector);
    }

}

