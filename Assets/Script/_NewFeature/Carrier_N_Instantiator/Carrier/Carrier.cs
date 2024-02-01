using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace Sophia.Instantiates
{
    public abstract class Carrier : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            OnTriggerLogic(other);
        }

        protected abstract void OnTriggerLogic(Collider entity);
    }
}