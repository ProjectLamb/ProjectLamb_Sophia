using System;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace Feature_NewData
{
    public class VisualFXObject : MonoBehaviour, IInstantiable<VisualFXObject, Entity>
    {

#region Serialize

        [SerializeField] private E_AFFECT_TYPE _affectType = E_AFFECT_TYPE.None;
        [SerializeField] private E_INSTANTIATE_STACKING_TYPE _stackingType = E_INSTANTIATE_STACKING_TYPE.Stack;
        [SerializeField] private E_INSTANTIATE_POSITION_TYPE _positioningType = E_INSTANTIATE_POSITION_TYPE.Outer;
        [SerializeField] private bool _isLooping = false; // 파티클이 반복되는 녀석인가.
        [SerializeField] private float _baseSize = 1f;
        [SerializeField] private float _baseDurateTime = 5f; //파티클 기본 지속 시간

#endregion

#region Members

        public E_AFFECT_TYPE AffectType { get; private set; }
        public E_INSTANTIATE_STACKING_TYPE StackingType { get; private set; }
        public E_INSTANTIATE_POSITION_TYPE PositioningType { get; private set; }

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

        public ParticleSystem Particle;
        public ParticleSystem.MainModule mParticleModule;

#endregion

#region ObjectPool

        private IObjectPool<VisualFXObject> poolRefer { get; set; }
        public void SetPool(IObjectPool<VisualFXObject> pool)
        {
            poolRefer = pool;
        }

#endregion

        private void Awake()
        {
            if (TryGetComponent<ParticleSystem>(out Particle))
            {
                mParticleModule = Particle.main;
            }
            AffectType = _affectType;
            StackingType = _stackingType;
            PositioningType = _positioningType;
        }

        private void OnParticleSystemStopped()
        {
            if (!IsInitialized) { return; }
            poolRefer.Release(this);
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

        public VisualFXObject InitByObject(Entity owner, object[] objects)
        {
            throw new NotImplementedException();
        }

        public VisualFXObject SetScaleByRatio(float sizeRatio)
        {
            CurrentSize = _baseSize * sizeRatio;
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
            AffectType = E_AFFECT_TYPE.None;

            ClearEvents();

            OwnerRef = null;
            IsInitialized = false;

            this.transform.transform.parent = null;
            this.transform.localScale = Vector3.one;
            this.transform.rotation = Quaternion.identity;
            this.transform.position = Vector3.zero;
        }

#endregion

#region Event Adder

        private UnityAction OnActivated;
        public VisualFXObject AddOnActivatedEvent(UnityAction action)
        {
            OnActivated += action;
            return this;
        }
        private UnityAction OnDeActivated;
        public VisualFXObject AddOnDeActivatedEvent(UnityAction action)
        {
            OnDeActivated += action;
            return this;
        }
        private UnityAction OnRelease;
        public VisualFXObject AddOnReleaseEvent(UnityAction action)
        {
            OnRelease += action;
            return this;
        }

        public void ClearEvents()
        {
            OnActivated = null;
            OnDeActivated = null;
            OnRelease = null;

            OnActivated ??= () => { };
            OnDeActivated ??= () => { };
            OnRelease ??= () => { };
        }

#endregion

#region Bindings

        public void Get()
        {
            this.Particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke();
            IsActivated = true;
            return;
        }

        public void DeActivate()
        {
            IsActivated = false;
            OnDeActivated?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        public void Release()
        {
            IsActivated = false;
            OnRelease?.Invoke();
            ResetSettings();
            gameObject.SetActive(false);
            return;
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