using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
namespace Feature_NewData
{
    public class TEST_CoolTimeUI : MonoBehaviour
    {
        public UnityEngine.UI.Image fill;
        public TextMeshProUGUI textMeshPro;
        
        private int BaseStackCounts; 
        public int StackCounts; 

        public CoolTimeManager coolTimeManager {get; private set;}

        private void Awake() {
            BaseStackCounts = StackCounts;
            coolTimeManager = new CoolTimeManager(5f, StackCounts, true);
            coolTimeManager
                .AddOnTickingEvent(UpdateFillAmount)
                .AddOnUseEvent(UseStack)
                .AddOnFinishedEvent(RecoverStack)
                .AddOnInitialized(ResetUI);
        }

        private void OnEnable() {
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = StackCounts.ToString();}));
        }

        [ContextMenu("Start")]
        public void TestCooldownUI(){
            coolTimeManager.DebugStart();
        }

        [ContextMenu("AccelerateFaster")] 
        public void TestAccelerateFaster() {
            coolTimeManager.SetAcceleratrion(2f);
        }


        [ContextMenu("AccelerateSlower")] 
        public void TestAccelerateSlower() {
            coolTimeManager.SetAcceleratrion(0.5f);
        }


        [ContextMenu("AccelerateReset")] 
        public void TestAccelerateReset() {
            coolTimeManager.SetAcceleratrion(1f);
        }

        [ContextMenu("Cool Down")] 
        public void TestAccelerateRemainByCurrentCoolTime() {
            coolTimeManager.AccelerateRemainByCurrentCoolTime(0.2f);
        }

        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - coolTimeManager.GetProgressAmount();
        }

            
        private void ResetUI() {
            fill.fillAmount = 0;
            StartCoroutine(DoAndRenderUI(() => {StackCounts = BaseStackCounts; textMeshPro.text = BaseStackCounts.ToString();}));
        }

        IEnumerator DoAndRenderUI(UnityAction action){
            action.Invoke(); yield return new WaitForEndOfFrame();
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