using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sophia;
using Sophia.Composite;
using System.Collections;
using UnityEngine.Events;

namespace Sophia.UserInterface
{
    /*********************************************************************************
    메이플과 같은 데미지 UI를 띄울것이다.

    그냥과 버퍼드 UI
        AlertUIManager;

    *********************************************************************************/
    public enum DamageInstant_Type
    {
        Stacking, Layerd
    }

    public class DamageTextInstantiator : MonoBehaviour
    {
        public LifeComposite LifeCompositeRef;
        [SerializeField]
        private DamageTextUI textUI;
        private DamageTextUI StakingText;
        private List<DamageTextUI> LayerdTextList;

        private void Start()
        {
            LifeCompositeRef ??= GetComponentInParent<ILifeAccessible>().GetLifeComposite();
            LifeCompositeRef.OnDamaged += Generate;
        }

        private void OnDisable()
        {
            LifeCompositeRef.OnDamaged -= Generate;
        }

        public void Generate(DamageInfo _damageInfo)
        {
                if (StakingText == null)
                {
                    StakingText = Instantiate(textUI, transform)
                                    .SetTextByString(_damageInfo.ToString())
                                    .SetAnimationSpeed(5f)
                                    .SetDestroyTimer(3f);
                    StakingText.ActivatedTextUI();
                }
                else
                {
                    StakingText.SetPosition(transform.position)
                                .ReactivateTextUI(_damageInfo);
                }
                Debug.Log("Gen");
        }
    }
}