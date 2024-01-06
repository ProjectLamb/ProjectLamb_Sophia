using UnityEngine;

namespace Feature_NewData
{
    public abstract class Carrier : MonoBehaviour
    {
        protected virtual void OnTriggerEnter(Collider other) {
            OnTriggerLogic(other);
        }

        protected abstract void OnTriggerLogic(Collider entity);
    }
}