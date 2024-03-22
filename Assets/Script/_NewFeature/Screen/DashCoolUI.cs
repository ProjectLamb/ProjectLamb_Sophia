using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using System;

namespace Sophia.UserInterface
{
    [Obsolete]
    public class DashCoolUI : MonoBehaviour
    {
        public UnityEngine.UI.Image fill;
        public TextMeshProUGUI textMeshPro;

        private Sophia.Composite.CoolTimeComposite TimerRef;

        public void SetTimer(Sophia.Composite.CoolTimeComposite CoolTimeComposite)
        {
            TimerRef = CoolTimeComposite;

            TimerRef.AddOnTickingEvent(UpdateFillAmount)
                    .AddOnUseEvent(UseStack)
                    .AddOnFinishedEvent(RecoverStack)
                    .AddOnInitialized(ResetUI);

            StartCoroutine(DoAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); }));
        }


        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - TimerRef.GetProgressAmount();
        }

        public void ResetUI()
        {
            fill.fillAmount = 0;
            StartCoroutine(DoAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.BaseStacksCount.ToString(); }));
        }

        public void DrawForce()
        {
            StartCoroutine(DoAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.BaseStacksCount.ToString(); }));
        }

        IEnumerator DoAndRenderUI(UnityAction action)
        {
            action.Invoke(); yield return new WaitForEndOfFrame();
        }

        private void UseStack()
        {
            StartCoroutine(DoAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); }));
        }
        private void RecoverStack()
        {
            StartCoroutine(DoAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); }));
        }
    }
}