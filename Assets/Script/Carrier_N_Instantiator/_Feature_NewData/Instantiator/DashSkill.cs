using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor.Rendering.Universal;
using UnityEditor.Searcher;
using UnityEditor.MPE;
using Newtonsoft.Json.Converters;
using FMODPlus;

namespace Feature_NewData
{
    public class DashSkill : IGlobalGameTimeUpdator{
        private Rigidbody rigidbodyRef;
        private FMODAudioSource DashSource;
        // private Stat MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural);
        // private Stat StaminaRestoreSpeed = new Stat(1, E_STAT_USE_TYPE.Ratio);

        
        public Stat MaxStamina {get; private set;}
        public Stat StaminaRestoreSpeed {get; private set;}
        public CoolTimeManager Timer {get; private set;}
        
        public bool IsDash {get;set;}

        public DashSkill(Rigidbody rb){
            rigidbodyRef = rb;
            MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural, OnMaxStaminaUpdated);
            StaminaRestoreSpeed = new Stat(1f, E_STAT_USE_TYPE.Ratio, OnStaminaRestoreSpeedUpdated);
            Timer = new CoolTimeManager(3f, MaxStamina.GetValueByNature(), true)
                .SetAcceleratrion(StaminaRestoreSpeed);
            
            GameObject.FindObjectOfType<DashCoolUI>().SetTimer(Timer);
            GlobalTimeUpdator.TimerContainer.Add(this);
        }

        private void OnMaxStaminaUpdated() {
            Timer.SetMaxStackCounts(this.MaxStamina.GetValueByNature());
        }

        private void OnStaminaRestoreSpeedUpdated() {
            Timer.SetAcceleratrion(this.StaminaRestoreSpeed.GetValueByNature());
        }

        public bool GetCanUseDash() {
            return !IsDash && this.Timer.GetIsReadyToUse();
        }

        public void SetAudioSource(FMODAudioSource source) { DashSource = source; }

        public void UseDashSkill(Vector3 moveVec, float moveSpeed) {
            if(IsDash) return;
            DashSource.Play();
            Vector3 dashPower = SetDashPower(moveVec, moveSpeed);
            Timer.ActionStart( () => {
                this.rigidbodyRef.AddForce(dashPower, ForceMode.VelocityChange);
            });
        }

        private Vector3 SetDashPower(Vector3 moveVec, float moveSpeed) {
            Vector3 temp = moveVec * -Mathf.Log(1 / this.rigidbodyRef.drag);
            return temp.normalized * PlayerDataManager.GetEntityData().MoveSpeed * 10;
        }

        public void LateTick() {return;}

        public void FrameTick()
        {
            Timer.Tick();
            return;
        }

        public void PhysicsTick() {
            if(rigidbodyRef.velocity.magnitude > PlayerDataManager.GetEntityData().MoveSpeed) { IsDash = true;}
            else {IsDash = false;}
            return;
        }
    }
}