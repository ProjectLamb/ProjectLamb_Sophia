using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.UI;

namespace Sophia.UserInterface
{
    public class ComboBarStateController : MonoBehaviour
    {
        [SerializeField] Michsky.UI.MTP.StyleManager styleManager;

        private void Start()
        {
            GameManager.Instance.NewFeatureGlobalEvent.OnEnemyHitEvent.AddListener(OnHitEventInLoop);
        }

        [ContextMenu("Start")]
        public void OnHitStart()
        {
            CurrentHitCounts = 1;
            styleManager.textItems[0].text = CurrentHitCounts.ToString();
            styleManager.textItems[0].UpdateText();
            styleManager.Play();
        }

        public void OnHitEventInLoop()
        {
            if (gameObject.activeSelf == false)
            {
                StartCoroutine(GlobalAsync.PerformAndRenderUI(OnHitStart)); return;
            }
            if (!styleManager.styleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
            {
                CurrentHitCounts++;
                styleManager.textItems[0].text = CurrentHitCounts.ToString();
                styleManager.textItems[0].UpdateText();
                styleManager.Play();
                styleManager.styleAnimator.Play("In", 0, 0.5f);
            }
        }

        public void OnHitExit() => CurrentHitCounts = 0;

        public int CurrentHitCounts = 0;
        public void SetCurrentHitCount(int Count)
        {
            CurrentHitCounts = Count;
            styleManager.textItems[0].text = CurrentHitCounts.ToString();
            styleManager.textItems[0].UpdateText();
            OnHitEventInLoop();
        }
    }

}
