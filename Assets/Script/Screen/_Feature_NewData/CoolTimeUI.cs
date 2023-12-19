using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
namespace Feature_NewData
{
    public class CoolTimeUI : MonoBehaviour
    {
        public UnityEngine.UI.Image fill;
        public TextMeshProUGUI textMeshPro;
        
        public int StackCounts; 

        public CoolTimeManager coolTimeManager;

        private void Awake() {
            coolTimeManager = new CoolTimeManager(5f, StackCounts, true);
            coolTimeManager
                .AddOnTickingEvent(UpdateFillAmount)
                .AddOnUseEvent(UseStack)
                .AddOnFinishedEvent(RecoverStack);
        }

        private void OnEnable() {
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = StackCounts.ToString();}));
        }

        [ContextMenu("Start")]
        public void TestCooldownUI(){
            coolTimeManager.DebugStart();
        }

        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - coolTimeManager.GetProgressAmount();
        }

        IEnumerator DoAndRenderUI(UnityAction action){
            action.Invoke();
            yield return new WaitForEndOfFrame();
        }

        private void UseStack() {
            StartCoroutine(DoAndRenderUI(() => {StackCounts--; textMeshPro.text = StackCounts.ToString();}));
        }
        private void RecoverStack() {
            StartCoroutine(DoAndRenderUI(() => {StackCounts++; textMeshPro.text = StackCounts.ToString();}));
        }

        // Test
        private void Update()
        {
            coolTimeManager.Tick();
        }
    }
}