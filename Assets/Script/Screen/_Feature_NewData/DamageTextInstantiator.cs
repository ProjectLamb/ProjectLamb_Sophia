using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Feature_NewData
{
    /*********************************************************************************
        메이플과 같은 데미지 UI를 띄울것이다.

        그냥과 버퍼드 UI
            AlertUIManager;

    *********************************************************************************/
    public class DamageTextInstantiator : MonoBehaviour{
        
        [SerializeField] 
        private DamageTextUI textUI;

        public void Generate(float _damageAmount){
            DamageTextUI InstanceOfDamageUI = Instantiate(textUI, transform)
                                                    .SetText(_damageAmount)
                                                    .SetAnimationSpeed(5f)
                                                    .SetDestroyTimer(1f);
            InstanceOfDamageUI.ActivatedTextUI();
            
            Destroy(InstanceOfDamageUI, InstanceOfDamageUI.DestroyTimer);
        }

    }
}