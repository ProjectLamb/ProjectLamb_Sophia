using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Pool;
using System.Collections;
using DG.Tweening;
using Sophia.Composite;
namespace Sophia.UserInterface
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
    public class DamageTextUI : MonoBehaviour
    {

        #region Members
        [SerializeField] Michsky.UI.MTP.TextItem _textItem;
        [SerializeField] private Rigidbody _rigid;
        public float AnimationSpeed { get; private set; }
        public float DestroyTimer { get; private set; }
        public int DamageAmount;

        #endregion

        #region Getter
        public float GetTextHeight()
        {
            return _textItem.textObject.maxHeight;
        }
        public Vector3 GetNextPosition()
        {
            return Vector3.up * 2;
        }
        #endregion

        #region Setter

        public DamageTextUI SetAnimationSpeed(float speed)
        {
            this.AnimationSpeed = speed;
            return this;
        }

        public DamageTextUI SetDestroyTimer(float time)
        {
            this.DestroyTimer = time;
            return this;
        }

        public DamageTextUI SetTextByString(string str)
        {
            if (_textItem.text.Length != 0) return this;
            _textItem.text = str;
            _textItem.UpdateText();
            return this;
        }
        public DamageTextUI SetText(float amount)
        {
            if (_textItem.text.Length != 0) return this;
            DamageAmount = (int)amount;
            _textItem.text = DamageAmount.ToString();
            return this;
        }


        public DamageTextUI SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        //      private IObjectPool<ProjectileObject> poolRefer {get; set;}
        //      public void SetPool(IObjectPool<ProjectileObject> pool) {
        //          poolRefer = pool;
        //      }

        #endregion

        private void Awake()
        {
            if (_rigid == null && TryGetComponent<Rigidbody>(out _rigid))
            {
                throw new System.Exception("RigidBody 컴포넌트 없음");
            }
        }

        Coroutine CurrentDestroyCoroutine;

        IEnumerator CoDestroy()
        {
            yield return YieldInstructionCache.WaitForSeconds(DestroyTimer);
            Destroy(this.gameObject);
        }

        public void ActivatedTextUI()
        {
            _rigid.velocity = Vector3.up * AnimationSpeed;
            CurrentDestroyCoroutine = StartCoroutine(CoDestroy());
        }
        
        IEnumerator DoAndRenderUI(UnityAction action)
        {
            action.Invoke(); 
            yield return new WaitForEndOfFrame();
        }

        public void ReactivateTextUI(DamageInfo damageInfo)
        {
            StopCoroutine(CurrentDestroyCoroutine);

            int currentDamage = DamageAmount;
            int newDamage = currentDamage + damageInfo.GetAmount();
            _rigid.velocity = Vector3.zero;
            Sequence ReactivateSeq = DOTween.Sequence();
            if (damageInfo.hitType == HitType.Critical)
            {
                Tween ShakeTween = transform.DOShakePosition(0.1f, 10);
                Tween CountTween = DOTween.To(() => currentDamage, x => currentDamage = x, newDamage, 0.1f)
                                        .OnUpdate(() => { _textItem.text = $"<b>!{currentDamage}!</b>"; _textItem.UpdateText(); DamageAmount = currentDamage; });
                ReactivateSeq.Append(ShakeTween).Join(CountTween).OnComplete(() => { _rigid.velocity = Vector3.up * AnimationSpeed; }).Play();
            }
            else if (damageInfo.damageHandleType == DamageHandleType.Dodge)
            {
                ReactivateSeq.OnComplete(() => { _textItem.text = $"<#666><i>Dodged<i></color>"; _textItem.UpdateText(); _rigid.velocity = Vector3.up * AnimationSpeed; }).Play();
            }
            else
            {
                Tween ShakeTween = transform.DOShakePosition(0.1f, 10);
                Tween CountTween = DOTween.To(() => currentDamage, x => currentDamage = x, newDamage, 0.1f)
                                        .OnUpdate(() => { _textItem.text = currentDamage.ToString(); _textItem.UpdateText(); DamageAmount = currentDamage; });
                ReactivateSeq.Append(ShakeTween).Join(CountTween).OnComplete(() => { _rigid.velocity = Vector3.up * AnimationSpeed; }).Play();
            }
            CurrentDestroyCoroutine = StartCoroutine(CoDestroy());
        }

        public void OnDestroy()
        {
            StopCoroutine(CurrentDestroyCoroutine);
        }
    }
}