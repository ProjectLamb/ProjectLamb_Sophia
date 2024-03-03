using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Sophia.Instantiates
{
    public abstract class Carrier : MonoBehaviour
    {
        public bool       _isDestroyable = true;
        public GameObject DestroyEffect = null;
        [HideInInspector] public Collider carrierCollider;
        [HideInInspector] public Rigidbody carrierRigidbody;
        protected virtual void Awake() {
            TryGetComponent<Collider>(out carrierCollider);
            TryGetComponent<Rigidbody>(out carrierRigidbody);
        }

        private void OnTriggerEnter(Collider other) {
            OnTriggerLogic(other);
        }

        protected abstract void OnTriggerLogic(Collider entity);
    }
}