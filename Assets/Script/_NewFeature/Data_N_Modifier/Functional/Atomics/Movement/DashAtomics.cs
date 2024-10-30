using System;
using UnityEngine;
using Sophia.Entitys;

namespace Sophia.DataSystem.Atomics
{
    public class DashAtomics
    {
        public readonly Func<(Vector3, int)> MovementDataFunction;
        public readonly Func<float> DashForceData;
        public readonly Rigidbody RigidRef;
        public DashAtomics(Rigidbody rigid, Func<(Vector3, int)> moveFunc, Func<float> forceFunc)
        {
            MovementDataFunction = moveFunc;
            DashForceData = forceFunc;
            RigidRef = rigid;
        }
        public void Invoke()
        {
            (Vector3 vec, int speed) moveData = this.MovementDataFunction.Invoke();
            Vector3 dashPower = (moveData.vec * -Mathf.Log(1 / this.RigidRef.drag)).normalized * DashForceData.Invoke();
            this.RigidRef.AddForce(dashPower, ForceMode.VelocityChange);
        }

        public void InvokeMousePoint()  //Only For Player
        {
            Vector3 moveData = Vector3.forward;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                moveData = hit.point - RigidRef.transform.position;
            }

            Vector3 dashPower = (moveData.normalized * -Mathf.Log(1 / this.RigidRef.drag)).normalized * DashForceData.Invoke();
            this.RigidRef.AddForce(dashPower, ForceMode.VelocityChange);
        }

        public bool CheckIsDashState()
        {
            (Vector3 vec, int speed) moveData = this.MovementDataFunction.Invoke();
            return RigidRef.velocity.magnitude > moveData.speed;
        }
    }
}