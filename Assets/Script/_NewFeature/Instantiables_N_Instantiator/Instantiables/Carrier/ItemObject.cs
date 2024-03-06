using UnityEngine.VFX;
using UnityEngine;

namespace Sophia.Instantiates
{
    using DG.Tweening;
    using Sophia.DataSystem;
    using Sophia.DataSystem.Modifiers;
    using Sophia.DataSystem.Modifiers.ConcreteEquipment;
    using Sophia.Entitys;
    using Sophia.UserInterface;

    public abstract class ItemObject : Carrier {

        [SerializeField] protected GameObject      _lootObject;
        [SerializeField] protected VisualEffect    _lootVFX;
        [SerializeField] protected bool           _isDestroyable = true;
        [SerializeField] protected GameObject     _destroyEffect = null;
        
        public bool IsInitialized       { get; protected set; }
        public bool IsActivated         { get; protected set; }
        public bool IsReadyToTrigger    { get; protected set; }
        public float TriggerTime         { get; protected set; }

        protected Sequence InstantiatedSequence;
        protected bool IsTweenActivated;
        [ContextMenu("Debug Activate")]
        public void DEBUG_Activate() {
            Init();Activate();
        }

        public ItemObject Init() {
            this.gameObject.SetActive(false);
            if (IsInitialized) { throw new System.Exception("이미 초기화가 됨."); }
            this.IsInitialized   = true;
            this.IsActivated = false;
            this.TriggerTime = 0;
            this.IsReadyToTrigger = true;
            if(_lootVFX != null) this._lootVFX?.Stop();
            return this;
        }

        public ItemObject SetTriggerTime(float triggerableTime) {
            this.TriggerTime = triggerableTime;
            this.IsReadyToTrigger = false;
            return this;
        }

        public ItemObject SetTweenSequence(Sequence sequence) {
            this.IsTweenActivated = true;
            this.InstantiatedSequence = sequence;
            return this;
        }

        public void Activate() {
            if(!IsInitialized) { throw new System.Exception("이미 초기화가 아직 안된상태에서 사용하려고 함."); }
            this.gameObject.SetActive(true);
            this.IsActivated = true;
            if(IsTweenActivated) {
                this.InstantiatedSequence.Append(DOVirtual.DelayedCall(0, () => {TriggerReady();}));
                this.InstantiatedSequence.Play();
            }
            else {
                TriggerReady();
            }
        }

        public void TriggerReady() {
            this.IsReadyToTrigger = true;
            if(_lootVFX != null) {
                this._lootVFX?.Play();
                Debug.Log(_lootVFX.name);
            }
        }
    }
}