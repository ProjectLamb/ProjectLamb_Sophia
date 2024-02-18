using UnityEngine;

namespace Sophia.DataSystem.Atomics
{
    public class TeleportAtomics {
        public readonly Transform transformRef;
        public readonly Vector3 teleportPos;
        public TeleportAtomics(Transform transform, ref Vector3 teleportPos) {
            transformRef = transform;
            this.teleportPos = teleportPos;
        }
        public void Invoke() {
            transformRef.position = teleportPos;
        }
        public void Revert() {
            transformRef.position = teleportPos;
        }
    }
}
