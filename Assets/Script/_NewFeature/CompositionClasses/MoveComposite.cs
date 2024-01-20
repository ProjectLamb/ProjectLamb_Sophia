using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using Sophia.DataSystem;

namespace Sophia.Composite
{
    public class MovementComposite 
    {
        public  Stat MoveSpeed {get; protected set;}
        public  Rigidbody RbRef {get; protected set;}

        public  Vector3     ForwardingVector {get; set;}
        public  Quaternion  Rotate {get; set;}
        private Vector2     mInputVec;

        public float CamRayLength {get; set;}
        public LayerMask GroundMask = LayerMask.GetMask("Wall", "Map");
        public LayerMask WallMask = LayerMask.GetMask("Wall");
        
        public MovementComposite(Rigidbody rigidbody, float baseMoveSpeed) {
            RbRef = rigidbody;
            MoveSpeed = new Stat(baseMoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, OnMoveSpeedUpdated);
        }

    #region Event

        public void OnMoveSpeedUpdated() {
            Debug.Log("이동속도 변경됨!");
            // throw new System.NotImplementedException();
        }
        public void SetInputVector(Vector2 vector) => mInputVec = vector;

        protected UnityAction<Vector3> OnMoveForward = null;
        public MovementComposite AddOnUpdateEvent(UnityAction<Vector3> action) {
            this.OnMoveForward += action;
            return this;
        }

    #endregion

    #region Getter

        public Vector3 GetForwardingVector() {
            Vector3 res = Vector3.zero;
            res += AngleToVector(Camera.main.transform.eulerAngles.y + 90f) * mInputVec.x;
            res += AngleToVector(Camera.main.transform.eulerAngles.y) * mInputVec.y;
            return res.normalized;
        }
        public bool IsBorder(Transform transform) {
            return Physics.Raycast(transform.position, ForwardingVector.normalized, 2, WallMask);
        }

        public (Vector3, int) GetMovemenCompositetData() => (GetForwardingVector(), MoveSpeed);
    
    #endregion

        public void Tick(Transform transform) {
            ForwardingVector = GetForwardingVector();
            this.RbRef.velocity = ForwardingVector * MoveSpeed.GetValueForce();

            if(ForwardingVector != Vector3.zero) {
                Rotate = Quaternion.LookRotation(ForwardingVector);
                transform.rotation = Quaternion.Slerp(transform.rotation,Rotate, 0.6f);
            }
            OnMoveForward?.Invoke(ForwardingVector);
        }

        public async UniTaskVoid TurningWithCallback(Transform transform, Vector3 mousePosition, UnityAction _turningCallback)
        {
            Ray camRay = Camera.main.ScreenPointToRay(mousePosition);

            // 레이캐스트 시작
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                Vector3 PlayerToPointerVector = groundHit.point - transform.position;
                PlayerToPointerVector.y = 0f;
                this.RbRef.MoveRotation(Quaternion.LookRotation(PlayerToPointerVector));
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
                _turningCallback?.Invoke();
            }
        }

        public async UniTaskVoid Turning(Transform transform, Vector3 mousePosition) {
            Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                Vector3 PlayerToPointerVector = groundHit.point - transform.position;
                PlayerToPointerVector.y = 0f;
                this.RbRef.MoveRotation(Quaternion.LookRotation(PlayerToPointerVector));
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }
        }
    
    #region Helper
        private Vector3 AngleToVector(float _angle) {
            _angle *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
        }

    #endregion
    }   
}