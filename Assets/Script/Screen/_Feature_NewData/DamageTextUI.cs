using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Pool;

namespace Feature_NewData
{    
    /*********************************************************************************
    
    데미지가 없으면 띄울것을 생각하고.

    우선순위
        1. 일단 데미지 UI가 띄워지는것
            자기자신이 애니메이션을 실행하면서 띄워지는것인가?
            ㄴㄴ Instantiatro에서 Init하고 생성한다. 

        2. 출현 애니메이션
            a. 일단은 RigidBody를 해서 띄우고
            b. Dotween을 사용
        
        3. 다양한 스킨
            치명타를 입혔는지 아닌지에 대한 특별한 스킨을 입히고
            Block되었는지 아닌지 하고.
            이 데미지의 타입이 뭔지에 따라서 다른 스킨을 띄울줄 알아야 한다.
        4. 옆에 몇콤보를 넣었는지 UI를 띄우는것이고

    의존성을 가지는것
        1. 스킨 그 자체를 나타내는 컴포넌트
        2. 오브젝트 풀을 담당하는녀석, Instantiator
        
    *********************************************************************************/
    public class DamageTextUI : MonoBehaviour {

#region Members
        private TextMeshProUGUI _tmpPro;
        private Rigidbody _rigid;
        public float AnimationSpeed {get; private set;}
        public float DestroyTimer {get; private set;}

#endregion

#region Setter

        public DamageTextUI SetAnimationSpeed(float speed ){
            this.AnimationSpeed = speed;
            return this;
        }

        public DamageTextUI SetDestroyTimer(float time) {
            this.DestroyTimer = time;
            return this;
        }

        public DamageTextUI SetText(float amount) {
            _tmpPro.text = ((int)amount).ToString();
            return this;
        }

//      private IObjectPool<Projectile> poolRefer {get; set;}
//      public void SetPool(IObjectPool<Projectile> pool) {
//          poolRefer = pool;
//      }

#endregion

        private void Awake() {
            if(TryGetComponent<TextMeshProUGUI>(out _tmpPro)) {
                throw new System.Exception("TextMeshProUGUI 컴포넌트 없음");
            }
            if(TryGetComponent<Rigidbody>(out _rigid)) {
                throw new System.Exception("RigidBody 컴포넌트 없음");
            }
        }

        private void Update() {
            
        }

        public void ActivatedTextUI() {
            _rigid.velocity = Vector3.up * AnimationSpeed;
        }

        public void OnDisable() {}
        public void OnDestroy() {}
        
//      private void OnBecameInvisible() {
//          poolRefer.Release(this);
//          this.transform.localScale = Vector3.one;
//          this.transform.rotation = Quaternion.identity;
//          this.Damage = 0;
//      }
    }
}