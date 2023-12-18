using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Feature_NewData
{
    public class DashSKill {
        private Rigidbody rigidbodyRef;
        
        // private Stat MaxStamina = new Stat(3, E_STAT_USE_TYPE.Natural);
        // private Stat StaminaRestoreSpeed = new Stat(1, E_STAT_USE_TYPE.Ratio);

        private float baseCoolTime;
        public float currentStamina;
        public float currentCoolTime;

        public bool IsDash {get;set;}
        
        private CancellationTokenSource cancel = new CancellationTokenSource();

        public DashSKill(Rigidbody rb, float baseCoolTime) {
            this.rigidbodyRef = rb;
            // this.currentStamina = MaxStamina;
        }

        public void Dash(Vector3 moveVec, float moveSpeed) {
            if(currentCoolTime > 0.01f || currentStamina <= 0) {return;}
            currentCoolTime = baseCoolTime;
            Vector3 dashPower = SetDashPower(moveVec, moveSpeed);
            this.rigidbodyRef.AddForce(dashPower, ForceMode.VelocityChange);
        }

        public async UniTask WaitCoolDown() {
            currentStamina--;
            currentCoolTime = baseCoolTime;
            while(currentCoolTime >= 0){
                currentCoolTime -= Time.deltaTime;
                await UniTask.Yield();
            }
            currentCoolTime = 0f;
            currentStamina++;
        }

        public Vector3 SetDashPower(Vector3 moveVec, float moveSpeed) {
            Vector3 temp = moveVec * -Mathf.Log(1 / this.rigidbodyRef.drag);
            return temp.normalized * PlayerDataManager.GetEntityData().MoveSpeed * 10;
        }

        public DashChecker() {
            
        }
    }
}