using System;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace Sophia.Instantiates
{
    using System.Globalization;
    using Sophia.Entitys;
    using Unity.Splines.Examples;
    using Unity.VisualScripting;

    /// <summary>
    /// VisualFXObject 생명주기
    ///     <para>
    ///         의존관계 : Client, VisualFXObjectPool, VisualFXBucket
    ///     </para>
    ///     <list type="bullet|number|table">
    ///         <item>
    ///             <term>0 : 풀 초기화</term>
    ///             <description>
    ///                 VisualFXObjectPool의 <br/>
    ///                 <c>VFXPool[Key:VisualFXObject's Name].Get();</c> 으로 풀에서 가져옴 <br/>
    ///                 Get()되는 순간 VisualFXObject에서<br/> <c>OnGetObject(VisualFXObject vfxObject);</c> 실행 <br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1 : 풀에서 인스턴스 가져오기</term>
    ///             <description>
    ///                 VisualFXObjectPool의 <br/>
    ///                 <c>VFXPool[Key:VisualFXObject's Name].Get();</c> 으로 풀에서 가져옴 <br/>
    ///                 Get()되는 순간 VisualFXObject에서<br/> <c>OnGetObject(VisualFXObject vfxObject);</c> 실행 <br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>2 : 초기화 이전 수행 작업</term>
    ///             <description>
    ///                 <c>GetByPool();</c>를 실행함으로<br/> 
    ///                 VisualFX는 일단 액티브 상태가 꺼진다. 그 이유는 초기화 단계가 아직 시작되지 않았기 떄문에다. <br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>3 : 초기화 시작</term>
    ///             <description>
    ///                 VisualFX를 사용하려고 호출하는 Client단에서 초기화가 진행된다 <br/>
    ///                 대표적인 Client는 대표적으로 Player이거나 Waepon <br/>
    ///                 초기화는 Setter 체이닝을통해 함수지향적으로 수행된다.<br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>4 : 인스턴스 활성화</term>
    ///             <description>
    ///                 초기화가 끝났다면 Client에서 Activate를 수행하여 활성화를 시작한다. <br/>
    ///                 이에 따라 <c>OnActivated.Invoke()</c> 가 수행된다. <br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>5 : 파티클 종료 핸들링</term>
    ///             <description>
    ///                 종료핸들링 방법 일반적으로 VisualFXObject는 Callback을 수행하며 종료되는데. <br/>바로 <c>private void OnParticleSystemStopped();</c>내부 로직을 수행한다.<br/>
    ///                 1 파티클의 재생이 끝났을떄 알아서 종료된다. <br/>
    ///                 2 강제로 파티클을 <c>DeActivate()</c>를 통해 <c>Stop()</c>했을때. <br/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>6 : 풀으로 릴리즈</term>
    ///             <description>
    ///                  종료하면서 실행되는 <c>ReleaseByPool</c> 에서 초기 세팅으로 되돌린다.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class VisualFXObject : MonoBehaviour, IPoolAccesable
    {

#region SerializeMember Member

        [SerializeField] private E_AFFECT_TYPE _affectType = E_AFFECT_TYPE.None;
        [SerializeField] private E_INSTANTIATE_STACKING_TYPE _stackingType = E_INSTANTIATE_STACKING_TYPE.Stack;
        [SerializeField] private E_INSTANTIATE_POSITION_TYPE _positioningType = E_INSTANTIATE_POSITION_TYPE.Outer;
        [SerializeField] private bool _isLooping = false; // 파티클이 반복되는 녀석인가.
        [SerializeField] private float _baseSize = 1f;
        [SerializeField] private float _baseDurateTime = 5f; //파티클 기본 지속 시간
        [SerializeField] public bool DEBUG;

        [ContextMenu("SetLoopOn")]
        private void SetLoopOn() => SetParticleLoopRecursivly(transform, true);
        
        [ContextMenu("SetLoopOff")]
        private void SetLoopOff() => SetParticleLoopRecursivly(transform, false);
        
#endregion

#region Members

        public E_AFFECT_TYPE AffectType { get{return this._affectType;} private set{} }
        public E_INSTANTIATE_STACKING_TYPE StackingType { get{return this._stackingType;} private set{} }
        public E_INSTANTIATE_POSITION_TYPE PositioningType { get{return this._positioningType;} private set{} }

        public Entity OwnerRef { get; private set; }

        private float mCurrentSize;
        public float CurrentSize
        {
            get { return mCurrentSize; }
            private set
            {
                if (value <= 0)
                {
                    mCurrentSize = 0;
                    transform.localScale = Vector3.zero;
                    return;
                }
                mCurrentSize = value;
                transform.localScale = Vector3.one * mCurrentSize;
            }
        }
        private float mCurrentDurateTime;
        public float CurrentDurateTime
        {
            get { return mCurrentDurateTime; }
            private set
            {
                if (mCurrentDurateTime <= 0.05f)
                {
                    mCurrentDurateTime = 0.05f;
                }
                mCurrentDurateTime = value;
            }
        }

        public bool IsInitialized { get; private set; }
        public bool IsActivated { get; private set; }

        public      ParticleSystem VisualFXParticle;
            
        public      ParticleSystem.MainModule       VisualFXMainModule;
        public      ParticleSystem.EmissionModule   ParticleEmissionModule;
        public      ParticleSystem.TriggerModule    ParticleTriggerModule;
        public      ParticleSystem.CollisionModule  ParticleColliderModule;

    #endregion

#region ObjectPool

        private IObjectPool<VisualFXObject> poolRefer { get; set; }
        public void SetByPool<T>(IObjectPool<T> pool) where T : MonoBehaviour
        {
            try
            {
                if(typeof(T).Equals(typeof(VisualFXObject))){
                    poolRefer = pool as IObjectPool<VisualFXObject>;
                    return;
                }
            }
            catch (System.Exception)
            {
            }
        }
        
        public void GetByPool()
        {
            gameObject.SetActive(false);
        }

        public void ReleaseByPool()
        {
            if(DEBUG){Debug.Log("릴리즈됨");}
            IsActivated = false;
            OnRelease?.Invoke();
            ResetSettings();
            gameObject.SetActive(false);
            return;
        }
        
        public event UnityAction OnActivated;
        public event UnityAction OnRelease;

        public void SetPoolEvents(UnityAction activated, UnityAction release)
        {
            OnActivated     = activated;

            OnRelease       = release;
        }

#endregion

        private void Awake()
        {
            VisualFXMainModule      = VisualFXParticle.main;
            ParticleEmissionModule  = VisualFXParticle.emission;
            ParticleTriggerModule   = VisualFXParticle.trigger;
            ParticleColliderModule  = VisualFXParticle.collision;

            AffectType = _affectType;
            
            if(this.DEBUG) Debug.Log($"Awake 실행 {this.AffectType}, {this.StackingType} {this.PositioningType}");
            
        }

        private void Start() {
            if(this.DEBUG) {Debug.Log(this.AffectType.ToString());}
            SetParticleLoopRecursivly(transform, _isLooping);
        }

        /// <summary>
        /// Monobehaviour 함수다.
        /// </summary>
        private void OnParticleSystemStopped() => DeActivate();
    
        private void SetParticleLoopRecursivly(Transform parent, bool isLoop) {
            if(parent.TryGetComponent<ParticleSystem>(out ParticleSystem particle)) {
                var mainModule = particle.main;
                mainModule.loop = isLoop;
                if(parent.childCount == 0) return;
                foreach(Transform child in parent) {
                    SetParticleLoopRecursivly(child, isLoop);
                }
            }
        }


#region Getter

        public Entity GetOwner() => OwnerRef;
        public bool GetIsInitialized() => IsInitialized;

#endregion

#region Setter

        public VisualFXObject Init(Entity owner)
        {
            if (GetIsInitialized() == true) { throw new System.Exception("이미 초기화가 됨."); }

            OwnerRef = owner;

            ClearEvents();

            IsInitialized = true;
            return this;
        }

        public VisualFXObject SetScaleOverrideByRatio(float sizeRatio)
        {
            CurrentSize = _baseSize * sizeRatio;
            return this;
        }

        public VisualFXObject SetScaleMultiplyByRatio(float sizeMulRatio)
        {
            CurrentSize *= sizeMulRatio;
            return this;
        }

        public VisualFXObject SetDurateTimeByRatio(float time)
        {
            CurrentDurateTime = _baseDurateTime * time;
            return this;
        }

        public VisualFXObject SetAffectType(E_AFFECT_TYPE affectType)
        {
            AffectType = affectType;
            return this;
        }

        private void ResetSettings()
        {
            if (IsActivated != false) { throw new Exception("아직 사용중임 리셋 불가능"); }

            CurrentSize = _baseSize;
            CurrentDurateTime = _baseDurateTime;
            AffectType = _affectType;

            ClearEvents();

            OwnerRef = null;
            IsInitialized = false;

            this.transform.parent = null;
            this.transform.localScale = Vector3.one;
            this.transform.rotation = Quaternion.identity;
            this.transform.position = Vector3.zero;
        }

#endregion

#region Event Adder
        public void ClearEvents()
        {
            OnActivated = null;

            OnRelease = null;

            OnActivated ??= () => { };

            OnRelease ??= () => { };
        }

#endregion

#region Bindings

        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke();
            IsActivated = true;
            return;
        }

        public void DeActivate() {
            if (!IsInitialized) { return; }
            poolRefer.Release(this);
        }


#endregion

#region Helper

        public bool CheckIsSameOwner(Entity owner)
        {
            return OwnerRef.Equals(owner);
        }
#endregion

    }
}