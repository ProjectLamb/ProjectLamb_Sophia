using UnityEngine;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using Sophia.Entitys;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Referer;
    using Sophia.Composite;

    public class ProjectileBucket : MonoBehaviour, IDataSetable
    {        

#region SerializeMember
        [SerializeField] private SerialBaseInstantiatorData _baseInstantiatorData;
        [SerializeField] private Entity _ownerRef;
        [SerializeField] private float _bucketScale = 1f;

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

        private void Awake() {
            Init(ref _baseInstantiatorData);
        }

        public void Init(ref SerialBaseInstantiatorData baseInstantiatorData) {
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

#endregion

#region Getter
        public float GetBucketSize() => _bucketScale * transform.lossyScale.z;
#endregion

#region Setter
#endregion

#region Event
        /*
        당연히 Functional로 관리를 해야 하지만 Projectil이 엄연히 다음 이벤트를 가지고 있는것은 사실이니.
        이벤트 주입을 하자.
        */
        
        public event UnityAction OnCreated = null;
        public event UnityAction OnTriggerd = null;
        public event UnityAction OnReleased = null;
        public event UnityAction OnForwarding = null;

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

#region Data Referer

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

#endregion

        public ProjectileObject InstantablePositioning(ProjectileObject instantiatedProjectile, Vector3 _offset)
        {
            Vector3     offset       = _offset;
            Vector3     position     = transform.position;
            Quaternion  forwardAngle = GetForwardingAngle(instantiatedProjectile.transform.rotation);
            instantiatedProjectile.transform.position = position;
            instantiatedProjectile.transform.rotation = forwardAngle;

            
            switch (instantiatedProjectile.PositioningType)
            {
                case E_INSTANTIATE_POSITION_TYPE.Inner   :
                {
                    instantiatedProjectile.transform.SetParent(transform);
                    instantiatedProjectile.SetScaleOverrideByRatio(transform.localScale.z);
                    break;
                }
                case E_INSTANTIATE_POSITION_TYPE.Outer  :
                {
                    break;
                }
            }

            switch (instantiatedProjectile.StackingType)
            {
                case E_INSTANTIATE_STACKING_TYPE.NonStack : 
                {
                    break;
                }
                case E_INSTANTIATE_STACKING_TYPE.Stack : 
                {
                    break;
                }
            }
            
            instantiatedProjectile.transform.position += offset * GetBucketSize();
            instantiatedProjectile.SetDurateTimeByRatio(InstantiableDurateLifeTimeMultiplyRatio.GetValueForce())
                                .SetScaleMultiplyByRatio(GetBucketSize())
                                .SetScaleMultiplyByRatio(InstantiableSizeMultiplyRatio.GetValueForce())
                                .SetForwardingSpeedByRatio(InstantiableForwardingSpeedMultiplyRatio.GetValueForce())
                                .SetOnProjectileCreatedEvent(OnCreated)
                                .SetOnProjectileForwardingEvent(OnTriggerd)
                                .SetOnProjectileReleasedEvent(OnReleased)
                                .SetOnProjectileTriggerdEvent(OnForwarding);
            
            return instantiatedProjectile;
        }

        public ProjectileObject InstantablePositioning(ProjectileObject instantiatable) => InstantablePositioning(instantiatable, instantiatable.transform.position);

        private Quaternion GetForwardingAngle(Quaternion instantiatorQuaternion)
        {
            return Quaternion.Euler(transform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }

        private Transform GetTransformParent(Transform instantiatorTransform)
        {
            instantiatorTransform.SetParent(this.transform);
            return instantiatorTransform;
        }
    }
}