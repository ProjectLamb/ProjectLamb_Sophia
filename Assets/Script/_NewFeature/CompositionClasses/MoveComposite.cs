using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using Sophia.DataSystem;
using Sophia.DataSystem.Referer;
using FMODPlus;
using DG.Tweening;
using System;

namespace Sophia.Composite
{
    public class MovementComposite : IDataSettable, IMovable
    {

        #region Data

        public Stat MoveSpeed { get; protected set; }
        public Extras<Vector3> MoveExtras { get; protected set; }
        public Extras<object> IdleExtras { get; protected set; }

        #endregion

        #region Member

        public Rigidbody RbRef { get; protected set; }
        public Transform TransformRef { get; protected set; }
        public bool IsMovable { get; protected set; }

        public Vector3 ForwardingVector;
        public Vector3 LastTouchedPointer;
        public Quaternion Rotate;
        private Vector2 mInputVec;
        public float BaseRotateSpeed = 0.25f;

        public const float CamRayLength = 500f;
        public LayerMask GroundMask = LayerMask.GetMask("Wall", "Map");
        public LayerMask WallMask = LayerMask.GetMask("Wall");
        private FMODAudioSource MoveSource;

        public MovementComposite(Transform t, Rigidbody rigidbody, float baseMoveSpeed)
        {
            this.TransformRef = t;
            this.RbRef = rigidbody;

            MoveSpeed = new Stat(baseMoveSpeed, E_NUMERIC_STAT_TYPE.MoveSpeed, E_STAT_USE_TYPE.Natural, OnMoveSpeedUpdated);
            MoveExtras = new Extras<Vector3>(E_FUNCTIONAL_EXTRAS_TYPE.Move, OnMoveExtrasUpdated);
            IdleExtras = new Extras<object>(E_FUNCTIONAL_EXTRAS_TYPE.Idle, OnIdleUpdated);

            IsMovable = true;
        }
        #endregion

        #region Event

        public void OnMoveSpeedUpdated() => Debug.Log("이동속도 변경됨!");
        public void OnMoveExtrasUpdated() => Debug.Log("이동 추가 동작 변경됨!");
        public void OnIdleUpdated() => Debug.Log("대기 추가 동작 변경됨!");

        public void SetInputVector(Vector2 vector) => mInputVec = vector;

        public event UnityAction<Vector3> OnMoveForward = null;

        #endregion

        #region Getter

        public bool GetMoveState() => IsMovable;

        public (Vector3, int) GetTouchedData()
        {
            if (Mathf.Abs(mInputVec.x) > 0.01 || Mathf.Abs(mInputVec.y) > 0.01)
            {
                return (LastTouchedPointer.normalized, MoveSpeed);
            }
            return (Vector3.zero, MoveSpeed);
        }
        public bool IsBorder(Transform transform)
        {
            return Physics.Raycast(transform.position, mInputVec.GetSophiaForwardingVector(), 2, WallMask);
        }

        public (Vector3, int) GetMovemenCompositetData() => (mInputVec.GetSophiaForwardingVector(), MoveSpeed);

        #endregion

        #region Setter 

        public void SetMoveState(bool Input) => IsMovable = Input;
        public void SetAudioSource(FMODAudioSource source) { MoveSource = source; }

        #endregion

        #region Data Referer 
        public void SetStatDataToReferer(EntityStatReferer statReferer)
        {
            statReferer.SetRefStat(MoveSpeed);
        }

        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
        {
            extrasReferer.SetRefExtras<Vector3>(MoveExtras);
            extrasReferer.SetRefExtras<object>(IdleExtras);
        }

        #endregion

        public void MoveTick()
        {
            if (!IsMovable) { return; }

            ForwardingVector = mInputVec.GetSophiaForwardingVector();
            this.RbRef.velocity = ForwardingVector * MoveSpeed.GetValueForce();

            if (ForwardingVector != Vector3.zero)
            {
                Rotate = Quaternion.LookRotation(ForwardingVector);
                TransformRef.rotation = Quaternion.Slerp(TransformRef.rotation, Rotate, 0.6f);
            }
            MoveExtras.PerformTickFunctionals(ref ForwardingVector);
            OnMoveForward?.Invoke(ForwardingVector);
        }

        public async UniTask Turning(Vector3 forwardingVector)
        {
            if (!IsMovable) { return; }

            Ray camRay = Camera.main.ScreenPointToRay(forwardingVector);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                LastTouchedPointer = groundHit.point - TransformRef.position;
                LastTouchedPointer.y = 0f;
                //this.RbRef.MoveRotation(Quaternion.LookRotation(LastTouchedPointer));
                RbRef.DORotate(Quaternion.LookRotation(LastTouchedPointer).eulerAngles, BaseRotateSpeed);
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }

        public async UniTask TurningWithAction(Transform transform, Vector3 forwardingVector, UnityAction action, bool latency)
        {
            if (!IsMovable) { return; }

            float rotateSpeed = BaseRotateSpeed;

            Ray camRay = Camera.main.ScreenPointToRay(forwardingVector);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                LastTouchedPointer = groundHit.point - transform.position;
                LastTouchedPointer.y = 0f;

                if (latency)
                    RbRef.DORotate(Quaternion.LookRotation(LastTouchedPointer).eulerAngles, rotateSpeed);
                else
                    RbRef.DORotate(Quaternion.LookRotation(LastTouchedPointer).eulerAngles, 0.05f);
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            action.Invoke();
        }

    }
}