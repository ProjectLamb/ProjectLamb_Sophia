using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using Sophia.DataSystem;
using Sophia.DataSystem.Referer;
using FMODPlus;
using DG.Tweening;

namespace Sophia.Composite
{
    public class MovementComposite : IDataSettable
    {

        #region Data

        public Stat MoveSpeed { get; protected set; }
        public Extras<Vector3> MoveExtras { get; protected set; }
        public Extras<object> IdleExtras { get; protected set; }

        #endregion

        #region Member

        public Rigidbody RbRef { get; protected set; }
        public bool IsMovable { get; protected set; }

        public Vector3 ForwardingVector;
        public Vector3 LastTouchedPointer;
        public Quaternion Rotate;
        private Vector2 mInputVec;

        public const float CamRayLength = 500f;
        public LayerMask GroundMask = LayerMask.GetMask("Wall", "Map");
        public LayerMask WallMask = LayerMask.GetMask("Wall");
        private FMODAudioSource MoveSource;

        public MovementComposite(Rigidbody rigidbody, float baseMoveSpeed)
        {

            RbRef = rigidbody;

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

        public void SetMovableState(bool Input) => IsMovable = Input;
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

        public void MoveTick(Transform transform)
        {
            if (!IsMovable) { return; }

            ForwardingVector = mInputVec.GetSophiaForwardingVector();
            this.RbRef.velocity = ForwardingVector * MoveSpeed.GetValueForce();

            if (ForwardingVector != Vector3.zero)
            {
                Rotate = Quaternion.LookRotation(ForwardingVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, Rotate, 0.6f);
            }
            MoveExtras.PerformTickFunctionals(ref ForwardingVector);
            OnMoveForward?.Invoke(ForwardingVector);
        }

        public async UniTask Turning(Transform transform, Vector3 mousePosition)
        {
            if (!IsMovable) { return; }

            Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                LastTouchedPointer = groundHit.point - transform.position;
                LastTouchedPointer.y = 0f;
                //this.RbRef.MoveRotation(Quaternion.LookRotation(LastTouchedPointer));
                RbRef.DORotate(Quaternion.LookRotation(LastTouchedPointer).eulerAngles, 0.1f);
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }
        public async UniTask TurningWithAction(Transform transform, Vector3 mousePosition, UnityAction action)
        {
            if (!IsMovable) { return; }

            Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(camRay, out RaycastHit groundHit, CamRayLength, GroundMask)) // 공격 도중에는 방향 전환 금지
            {
                LastTouchedPointer = groundHit.point - transform.position;
                LastTouchedPointer.y = 0f;
                //this.RbRef.MoveRotation(Quaternion.LookRotation(LastTouchedPointer));
                RbRef.DORotate(Quaternion.LookRotation(LastTouchedPointer).eulerAngles, 0.1f);
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            action.Invoke();
        }
    }
}