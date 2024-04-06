using UnityEngine;

namespace Sophia.DataSystem.Atomics
{
    public class TeleportAtomics {
        public readonly Transform transformRef;
        public Vector3 teleportPrevPos {get; private set;}
        public Vector3 teleportPos {get; private set;}
        public TeleportAtomics(Transform transform, ref Vector3 teleportPos) {
            transformRef = transform;
            this.teleportPos = teleportPos;
        }
        public void SetTeleportPos(Vector3 newVec) => teleportPos = newVec;

        public void Invoke() {
            teleportPrevPos = transformRef.position;
            transformRef.position = teleportPos;
        }
        public void Revert() {
            transformRef.position = teleportPrevPos;
        }
    }
}
