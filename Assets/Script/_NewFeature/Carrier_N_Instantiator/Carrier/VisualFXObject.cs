using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Events;

namespace Feature_NewData
{
    public enum E_INSTANTIATE_STACKING_TYPE {
        NonStack, Stak
    }
    public class VisualFXObject : MonoBehaviour, IInstantiable<VisualFXObject, Entity>
    {

#region Members

        public Entity OwnerRef          {get; private set;}
        private float mBaseSize;
        public float BaseSize        {
            get {return mBaseSize;} 
            private set {
                if(value <= 0) {
                    mBaseSize = 0;
                    transform.localScale = Vector3.zero;
                    return;
                }
                mBaseSize = value;
                transform.localScale = Vector3.one * mBaseSize;
            }
        }

        private float mBaseDurateTime;
        public float BaseDurateTime  {
            get {return mBaseDurateTime;} 
            private set {
                if(mBaseDurateTime <= 0.05f) {
                    mBaseDurateTime = 0.05f;
                }
                mBaseDurateTime = value;
            }
        }

        public bool  IsCloned           {get; private set;} // 프리펩이 아닌 Instantiated된 객체인가?
        public bool  IsLooping          {get; private set;} // 파티클이 반복되는 녀석인가.
        
        public E_INSTANTIATE_STACKING_TYPE StackingType {get; private set;}
        public E_AFFACT_TYPE AffactType {get; private set;}

        [SerializeField]
        private ParticleSystem MainParticle;
        
        private ParticleSystem.MainModule   mParticleModule;

#endregion

        private void Awake() {
            TryGetComponent<ParticleSystem>(out MainParticle);
        }

        /*기껏 Clone하고 비활성화 하고 이벤트 다 박았는데 OnEnable하자마자 이벤트 싹다 사라지는 버그 생길듯*/
//      private void OnEnable() {
//          OnActivated     ??= () => {};
//          OnDeActivated   ??= () => {};
//          OnRelesed       ??= () => {};
//      }

//      private void OnDisable() {
//          OnActivated     = null;
//          OnDeActivated   = null;
//          OnRelesed       = null;
//      }

#region Clone by prefeb instantiate
        
        public VisualFXObject Clone()
        {
            if(this.IsCloned == true) throw new System.Exception("복제본이 복제본을 만들 수 는 없다.");
            VisualFXObject res = Instantiate(this);
            res.gameObject.SetActive(false);
            res.IsCloned = true;
            return res;
        }

#endregion

#region Getter

        public Entity GetOwner()
        {
            return this.OwnerRef;
        }

#endregion

#region Setter

        public VisualFXObject Init(Entity owner)
        {
            return this;
        }

        public VisualFXObject InitByObject(Entity owner, object[] objects)
        {
            return this;
        }

        public VisualFXObject SetPositionAndForwarding(Transform transform, Quaternion instantiatorAngle)
        {
            this.transform.SetPositionAndRotation(transform.position, instantiatorAngle);
            return this;
        }

        public VisualFXObject SetScale(float sizeRatio)
        {
            this.BaseSize = sizeRatio;
            return this;
        }

        public VisualFXObject SetDurateTime(float time)
        {
            this.BaseDurateTime = time;
            return this;
        }

        public VisualFXObject SetStackingType (E_INSTANTIATE_STACKING_TYPE stackingTpye) {
            this.StackingType = stackingTpye;
            return this;
        }

        public VisualFXObject SetAffactType (E_AFFACT_TYPE affactType){
            this.AffactType = affactType;
            return this;
        }

#endregion

#region Event Adder

        private UnityAction OnActivated;
        public VisualFXObject AddOnOnActivatedEvent(UnityAction action) {
            OnActivated += action;
            return this;
        }
        private UnityAction OnDeActivated;
        public VisualFXObject AddOnOnDeActivatedEvent(UnityAction action) {
            OnDeActivated += action;
            return this;
        }
        private UnityAction OnRelesed;
        public VisualFXObject AddOnOnRelesedEvent(UnityAction action) {
            OnRelesed += action;
            return this;
        }

        public void ClearEvents() {
            OnActivated     = null;
            OnDeActivated   = null;
            OnRelesed       = null;

            OnActivated     ??= () => {};
            OnDeActivated   ??= () => {};
            OnRelesed       ??= () => {};
        }

#endregion

#region Bindings

        public void Activate() 
        {
            OnActivated?.Invoke();
            this.gameObject.SetActive(true);
            return;
        }
        
        public void DeActivate()
        {
            OnDeActivated?.Invoke();
            this.gameObject.SetActive(false);
            return;
        }

        public void Release()
        {
            OnRelesed?.Invoke();
            Destroy(this.gameObject);
            return;
        }

#endregion

#region Helper

        public bool CheckIsSameOwner(Entity owner)
        {
            return this.OwnerRef.Equals(owner);
        }

        public bool CheckIsCloned()
        {
            return this.IsCloned;
        }

        #endregion

    }
}