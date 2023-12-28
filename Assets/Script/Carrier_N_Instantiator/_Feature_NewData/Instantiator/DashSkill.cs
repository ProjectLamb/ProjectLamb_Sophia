using UnityEngine;
using FMODPlus;

namespace Feature_NewData
{
    public class DashSkill : IUpdatable {

#region Members 
        private Rigidbody rigidbodyRef;
        private FMODAudioSource DashSource;
        // private Stat MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural);
        // private Stat StaminaRestoreSpeed = new Stat(1, E_STAT_USE_TYPE.Ratio);

        
        public Stat MaxStamina {get; private set;}
        public Stat StaminaRestoreSpeed {get; private set;}
        public CoolTimeManager Timer {get; private set;}
        public DashCoolUI DashUI {get; private set;}

        public DashSkill(Rigidbody rb){
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
            
            Timer = new CoolTimeManager(3f, MaxStamina.GetValueByNature())
                .SetAcceleratrion(StaminaRestoreSpeed);
            
            DashUI = GameObject.FindObjectOfType<DashCoolUI>();
            DashUI.SetTimer(Timer);
            
            GlobalTimeUpdator.CheckAndAdd(this);
        }

# endregion

#region Getter
        
        public bool GetIsDashState(float moveSpeed) {
            return (rigidbodyRef.velocity.magnitude - 0.05f * moveSpeed) > moveSpeed;
        }

#endregion

#region Setter

        public void SetAudioSource(FMODAudioSource source) { DashSource = source; }
        
        private Vector3 SetDashPower(Vector3 moveVec, float moveSpeed) {
            Vector3 temp = moveVec * -Mathf.Log(1 / this.rigidbodyRef.drag);
            return temp.normalized * PlayerDataManager.GetEntityData().MoveSpeed * 10;
        }

        public void SetDashCoolUIRefer(DashCoolUI UIRef) {
            DashUI = UIRef;
            DashUI.SetTimer(Timer);
        }

#endregion

        private void OnMaxStaminaUpdated() {
            Timer.SetMaxStackCounts(this.MaxStamina.GetValueByNature());
            DashUI.ResetUI();
        }

        private void OnStaminaRestoreSpeedUpdated() {
            Timer.SetAcceleratrion(this.StaminaRestoreSpeed.GetValueByNature());
            DashUI.ResetUI();
        }

        public void UseDashSkill(Vector3 moveVec, float moveSpeed) {
            Debug.Log(moveSpeed);
            if( !GetIsDashState(moveSpeed) && rigidbodyRef.velocity.magnitude > 0.001f &&Timer.GetIsReadyToUse() ){
                DashSource.Play();
                Vector3 dashPower = SetDashPower(moveVec, moveSpeed);
                Timer.ActionStart( () => {
                    this.rigidbodyRef.AddForce(dashPower, ForceMode.VelocityChange);
                });
            }
        }

#region  Implements

        public void LateTick() {return;}

        public void FrameTick() {
            Timer.Tick(); return;
        }

        public void PhysicsTick() { return; }

#endregion

    }
}