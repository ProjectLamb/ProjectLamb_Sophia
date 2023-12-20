using UnityEngine;
using FMODPlus;

namespace Feature_NewData
{
    public class DashSkill : IUpdatable {
        private Rigidbody rigidbodyRef;
        private FMODAudioSource DashSource;
        // private Stat MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural);
        // private Stat StaminaRestoreSpeed = new Stat(1, E_STAT_USE_TYPE.Ratio);

        
        public Stat MaxStamina {get; private set;}
        public Stat StaminaRestoreSpeed {get; private set;}
        public CoolTimeManager Timer {get; private set;}
        public DashCoolUI DashUI;

        public DashSkill(Rigidbody rb){
            rigidbodyRef = rb;
            MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural, OnMaxStaminaUpdated);
            StaminaRestoreSpeed = new Stat(1f, E_STAT_USE_TYPE.Ratio, OnStaminaRestoreSpeedUpdated);
            
            Timer = new CoolTimeManager(3f, MaxStamina.GetValueByNature())
                .SetAcceleratrion(StaminaRestoreSpeed);
            
            DashUI = GameObject.FindObjectOfType<DashCoolUI>();
            DashUI.SetTimer(Timer);
            
            GlobalTimeUpdator.CheckAndAdd(this);
        }

        private void OnMaxStaminaUpdated() {
            Timer.SetMaxStackCounts(this.MaxStamina.GetValueByNature());
            DashUI.ResetUI();
        }

        private void OnStaminaRestoreSpeedUpdated() {
            Timer.SetAcceleratrion(this.StaminaRestoreSpeed.GetValueByNature());
            DashUI.ResetUI();
        }

        public void SetAudioSource(FMODAudioSource source) { DashSource = source; }

        public void UseDashSkill(Vector3 moveVec, float moveSpeed) {
            if( !GetIsDashState(moveSpeed) && Timer.GetIsReadyToUse() ){
                DashSource.Play();
                Vector3 dashPower = SetDashPower(moveVec, moveSpeed);
                Timer.ActionStart( () => {
                    this.rigidbodyRef.AddForce(dashPower, ForceMode.VelocityChange);
                });
            }
        }

        private Vector3 SetDashPower(Vector3 moveVec, float moveSpeed) {
            Vector3 temp = moveVec * -Mathf.Log(1 / this.rigidbodyRef.drag);
            return temp.normalized * PlayerDataManager.GetEntityData().MoveSpeed * 10;
        }

        public void LateTick() {return;}

        public void FrameTick() {
            Timer.Tick(); return;
        }

        public void PhysicsTick() { return; }

        public bool GetIsDashState(float moveSpeed) {
            return (rigidbodyRef.velocity.magnitude - 0.05f * moveSpeed) > moveSpeed;
        }
    }
}