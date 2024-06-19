using UnityEngine;
using FMODPlus;
using Sophia.DataSystem;
using System.Numerics;
using System;

using Vector3 = UnityEngine.Vector3;
using Sophia.UserInterface;
using Sophia.DataSystem.Referer;
using UnityEngine.Events;

namespace Sophia.Composite
{
    public class DashSkill : IUpdatorBindable, IDataSettable
    {

        #region Members 

        public Stat MaxStamina { get; private set; }
        public Stat StaminaRestoreSpeed { get; private set; }
        public Stat DashForce { get; private set; }
        public Extras<object> DashExtras { get; private set; }

        private Rigidbody rigidbodyRef;
        private FMODAudioSource DashSource;

        public CoolTimeComposite Timer { get; private set; }
        public DataSystem.Atomics.DashAtomics DashAtomics { get; private set; }

        public DashSkill(Rigidbody rb, Func<(Vector3, int)> movementDataSender, float forceAmount)
        {
            rigidbodyRef = rb;
            MaxStamina = new Stat(3,
                E_NUMERIC_STAT_TYPE.MaxStamina,
                E_STAT_USE_TYPE.Natural,
                OnMaxStaminaUpdated
            );
            StaminaRestoreSpeed = new Stat(1f,
                E_NUMERIC_STAT_TYPE.StaminaRestoreSpeed,
                E_STAT_USE_TYPE.Ratio,
                OnStaminaRestoreSpeedUpdated
            );

            DashForce = new Stat(forceAmount,
                E_NUMERIC_STAT_TYPE.DashForce,
                E_STAT_USE_TYPE.Natural,
                OnDashForceUpdate
            );

            DashExtras = new Extras<object>(
                E_FUNCTIONAL_EXTRAS_TYPE.Dash,
                OnDashExtrasUpdated
            );

            DashAtomics = new DataSystem.Atomics.DashAtomics(rigidbodyRef, movementDataSender, DashForce.GetValueForce);

            Timer = new Sophia.Composite.CoolTimeComposite(3f, MaxStamina)
                .SetAcceleratrion(StaminaRestoreSpeed)
                .AddBindingAction(Dash);

            AddToUpdater();
        }


        #endregion

        #region Getter

        public bool GetIsDashState() => DashAtomics.CheckIsDashState();

        #endregion

        #region Setter

        public void SetAudioSource(FMODAudioSource source) { DashSource = source; }
        public void SetDependUI(PlayerStaminaBarUI playerStaminaBarUI)
        {
            playerStaminaBarUI.SetReferenceComposite(this);
        }

        #endregion

        #region Event

        private void OnMaxStaminaUpdated()
        {
            Timer.SetMaxStackCounts(this.MaxStamina);
            InGameScreenUI.Instance._playerStaminaBarUI.MaxStaminaUpdatedHandler();
        }

        private void OnStaminaRestoreSpeedUpdated()
        {
            Timer.SetAcceleratrion(this.StaminaRestoreSpeed);
        }

        private void OnDashForceUpdate()
        {
            Debug.Log("대쉬 속도 갱신됨");
        }

        private void OnDashExtrasUpdated()
        {
            Debug.Log("대쉬 추가 동작 변경됨!");
        }

        #endregion

        #region Data Referer 

        public void SetStatDataToReferer(EntityStatReferer statReferer)
        {
            statReferer.SetRefStat(MaxStamina);
            statReferer.SetRefStat(StaminaRestoreSpeed);
            statReferer.SetRefStat(DashForce);
        }
        public void SetExtrasDataToReferer(EntityExtrasReferer extrasReferer)
        {
            extrasReferer.SetRefExtras<object>(DashExtras);
        }

        #endregion

        #region Updator Implements

        public void LateTick() { return; }

        public void FrameTick() { Timer.TickRunning(); }

        public void PhysicsTick() { return; }

        bool IsUpdatorBinded = false;
        public bool GetUpdatorBind() => IsUpdatorBinded;

        public void AddToUpdater()
        {
            GlobalTimeUpdator.CheckAndAdd(this);
            IsUpdatorBinded = true;
        }

        public void RemoveFromUpdator()
        {
            GlobalTimeUpdator.CheckAndRemove(this);
            IsUpdatorBinded = false;
        }

        #endregion

        public void Dash()
        {
            DashAtomics.Invoke();
            DashSource.Play();
        }

        public void Use()
        {
            if ((!GetIsDashState() && rigidbodyRef.velocity.magnitude >= 0.01f) && Timer.GetIsReadyToUse())
            {
                Timer.ActionStart();
                if (rigidbodyRef.gameObject.layer == LayerMask.NameToLayer("PlayerEntity"))
                {
                    rigidbodyRef.gameObject.layer = LayerMask.NameToLayer("Dash");
                    GameManager.Instance.CameraController.StartDashDamping();
                }
            }
        }

        public void RecoverOneDash() => Timer.stackCounter.RecoverStack();

    }
}