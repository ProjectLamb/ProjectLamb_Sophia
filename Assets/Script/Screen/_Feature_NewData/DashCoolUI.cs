using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

namespace Feature_NewData
{
    public class DashCoolUI : MonoBehaviour
    {
        public UnityEngine.UI.Image fill;
        public TextMeshProUGUI textMeshPro;
        
        private CoolTimeManager Timer;

        public void SetTimer(CoolTimeManager coolTimeManager) {
            Timer = coolTimeManager;

            Timer.AddOnTickingEvent(UpdateFillAmount)
                    .AddOnUseEvent(UseStack)
                    .AddOnFinishedEvent(RecoverStack)
                    .AddOnInitialized(ResetUI);
            
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = Timer.CurrentStacksCount.ToString();}));
        }


        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - Timer.GetProgressAmount();
        }
            
        public void ResetUI() {
            fill.fillAmount = 0;
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = Timer.BaseStacksCount.ToString();}));
        }

        public void DrawForce() {
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = Timer.BaseStacksCount.ToString();}));
        }

        IEnumerator DoAndRenderUI(UnityAction action){
            action.Invoke(); yield return new WaitForEndOfFrame();
        }

        private void UseStack() {
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = Timer.CurrentStacksCount.ToString();}));
        }
        private void RecoverStack() {
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = Timer.CurrentStacksCount.ToString();}));
        }
    }
}