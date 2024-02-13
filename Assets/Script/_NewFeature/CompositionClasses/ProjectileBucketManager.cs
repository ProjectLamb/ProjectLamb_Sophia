using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite;
    using System.Linq;

    public class ProjectileBucketManager : MonoBehaviour, IDataSettable
    {      
#region SerializeMember

        [SerializeField] private SerialBaseInstantiatorData _baseInstantiatorData;
        [SerializeField] private Entity _ownerRef;
        [SerializeField] private ProjectileBucket[] _projectileBuckets;

#endregion

#region Member

        public Stat InstantiableDurateLifeTimeMultiplyRatio {get; protected set;}
        public Stat InstantiableSizeMultiplyRatio {get; protected set;}
        public Stat InstantiableForwardingSpeedMultiplyRatio {get; protected set;}

        public Extras<Entity> ConveyAffectExtras    {get; protected set;}
        public Extras<object> AttackExtras          {get; protected set;}
        public Extras<object> CreatedExtras         {get; protected set;}
        public Extras<object> TriggerdExtras        {get; protected set;}
        public Extras<object> ReleasedExtras        {get; protected set;}
        public Extras<object> ForwardingExtras      {get; protected set;}

#endregion

        private void Awake() {
            Init(in _baseInstantiatorData);
        }

        public void Init(in SerialBaseInstantiatorData baseInstantiatorData) {
            InstantiableDurateLifeTimeMultiplyRatio = new Stat(baseInstantiatorData.InstantiableDurateLifeTimeMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableDurateLifeTimeMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnDurateLifeTimeUpdated
            );
            InstantiableSizeMultiplyRatio = new Stat(baseInstantiatorData.InstantiableSizeMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableSizeMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnRatioSizeUpdated
            );
            InstantiableForwardingSpeedMultiplyRatio = new Stat(baseInstantiatorData.InstantiableForwardingSpeedMultiplyRatio,
                E_NUMERIC_STAT_TYPE.InstantiableForwardingSpeedMultiplyRatio,
                E_STAT_USE_TYPE.Ratio, OnForwardingSpeedUpdated
            );

            ConveyAffectExtras  = new Extras<Entity>(E_FUNCTIONAL_EXTRAS_TYPE.ConveyAffect, OnConveyAffectUpdated);
            AttackExtras        = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Attack, OnAttackUpdated);
            CreatedExtras       = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Created, OnCreatedUpdated);
            TriggerdExtras      = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Triggerd, OnTriggerdUpdated);
            ReleasedExtras      = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Released, OnReleasedUpdated);
            ForwardingExtras    = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Forwarding, OnForwardingUpdated);

            OnCreated       ??= () => {};
            OnTriggerd      ??= () => {};
            OnReleased      ??= () => {};
            OnForwarding    ??= () => {};
        }

        private void Start() {
            for(int i = 0 ; i < _projectileBuckets.Count(); i ++) {
                _projectileBuckets[i]?.SetProjectileBucketManamger(this);
            }
        }

#region Event
        /*
        당연히 Functional로 관리를 해야 하지만 Projectil이 엄연히 다음 이벤트를 가지고 있는것은 사실이니.
        이벤트 주입을 하자.
        */
        
        public UnityAction OnCreated = null;
        public UnityAction OnTriggerd = null;
        public UnityAction OnReleased = null;
        public UnityAction OnForwarding = null;

        protected void OnConveyAffectUpdated()      => Debug.Log("Affect Extras Changed");
        protected void OnDurateLifeTimeUpdated()    => Debug.Log("투사체 유지시간 변경");
        protected void OnRatioSizeUpdated()         => Debug.Log("투사체 사이즈 변경");
        protected void OnForwardingSpeedUpdated()   => Debug.Log("투사체 속도 변경");
        protected void OnAttackUpdated()            => Debug.Log("투사체 속도 변경");
        protected void OnCreatedUpdated()           => Debug.Log("투사체 생성 추가 변경");
        protected void OnTriggerdUpdated()          => Debug.Log("투사체 접촉시 추가 변경");
        protected void OnReleasedUpdated()          => Debug.Log("투사체 파괴 추가 변경");
        protected void OnForwardingUpdated()        => Debug.Log("투사체 진행시 추가 변경");
        
#endregion

        public void SetStatDataToReferer(EntityStatReferer statReferer)
        {
            statReferer.SetRefStat(InstantiableDurateLifeTimeMultiplyRatio);
            statReferer.SetRefStat(InstantiableSizeMultiplyRatio);
            statReferer.SetRefStat(InstantiableForwardingSpeedMultiplyRatio);
        }

        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
        {
            extrasReferer.SetRefExtras<Entity>(ConveyAffectExtras);
            extrasReferer.SetRefExtras<object>(AttackExtras);
            extrasReferer.SetRefExtras<object>(CreatedExtras);
            extrasReferer.SetRefExtras<object>(TriggerdExtras);
            extrasReferer.SetRefExtras<object>(ReleasedExtras);
            extrasReferer.SetRefExtras<object>(ForwardingExtras);
        }

#region Instantiate

        public ProjectileObject InstantablePositioning(int index, ProjectileObject instantiatedProjectile, Vector3 _offset) => _projectileBuckets[index].InstantablePositioning(instantiatedProjectile, _offset);
        public ProjectileObject InstantablePositioning(int index, ProjectileObject instantiatedProjectile) => _projectileBuckets[index].InstantablePositioning(instantiatedProjectile);

#endregion
    }
}