using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Sophia.Instantiates
{
    public abstract class Carrier : MonoBehaviour
    {
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