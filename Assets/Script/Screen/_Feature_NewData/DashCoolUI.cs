using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using JetBrains.Annotations;

namespace Feature_NewData
{
    public class DashCoolUI : MonoBehaviour, IUpdatable
    {
        public UnityEngine.UI.Image fill;
        public TextMeshProUGUI textMeshPro;
        
        private int BaseStackCounts; 
        public int StackCounts; 
        
        private CoolTimeManager Timer;

        private void OnEnable() {
        }

        public void SetTimer(CoolTimeManager coolTimeManager) {
            Timer = coolTimeManager;

            Timer.AddOnTickingEvent(UpdateFillAmount)
                    .AddOnUseEvent(UseStack)
                    .AddOnFinishedEvent(RecoverStack)
                    .AddOnInitialized(ResetUI);
            
            BaseStackCounts = Timer.BaseStacksCount;
            StackCounts     = Timer.CurrentStacksCount;

            GlobalTimeUpdator.CheckAndAdd(this);
            
            StartCoroutine(DoAndRenderUI(() => {textMeshPro.text = StackCounts.ToString();}));
        }


        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - Timer.GetProgressAmount();
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

        public void LateTick() { Timer.Tick(); }

        public void FrameTick(){return;}

        public void PhysicsTick(){return;}
    }
}