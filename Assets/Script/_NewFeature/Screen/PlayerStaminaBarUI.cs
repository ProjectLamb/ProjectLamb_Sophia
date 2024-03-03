using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Sophia.Composite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sophia.DataSystem;

namespace Sophia.UserInterface
{
    public class PlayerStaminaBarUI : MonoBehaviour, IHealthBarUI<DashSkill> {
        [SerializeField] Slider _dashSliderPrefeb;
        [SerializeField][ColorUsage(true)] Color _chargingColor;
        [SerializeField][ColorUsage(true)] Color _existColor;

        private DashSkill dashSkillRef;
        private List<Slider> sliders = new();
        private Stack<Slider> existsliders = new Stack<Slider>();
        private Stack<Slider> chargingSlider = new Stack<Slider>();
        private float progressOneSlider = 0;
        public void SetReferenceComposite(DashSkill dashSkill)
        {
            dashSkillRef = dashSkill;
            dashSkillRef.Timer.AddOnInitialized(InitializedHandler)
                                    .AddOnStartCooldownEvent(StartCooldownEventHandler)
                                    .AddOnTickingEvent(TickingEventHandler)
                                    .AddOnIntervalEvent(IntervalEventHandler)
                                    .AddOnFinishedEvent(FinishedEventHandler)
                                    .AddOnUseEvent(UseEventHandler)
                                    .AddOnRecoverEvent(RecoverEventHandler);
            foreach(Transform child in transform){ Destroy(child.gameObject); }
            for(int i = 0 ; i < dashSkillRef.Timer.stackCounter.CurrentStacksCount; i++) {
                existsliders.Push(Instantiate(_dashSliderPrefeb, transform));
            } 
        }
        private void MaxStaminaUpdatedHandler() {
            int maxStamina = dashSkillRef.MaxStamina;
            foreach(Transform child in transform){ Destroy(child.gameObject); }
            for(int i = 0 ; i < maxStamina; i++) {
                existsliders.Push(Instantiate(_dashSliderPrefeb, transform));
            } 
        }

        private void InitializedHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                sliders.ForEach(E => E.value = 1f);
            }));
        }
        
        private void StartCooldownEventHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {}));
        }
        
        // UpdateFillAmount
        private void TickingEventHandler(float input) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                progressOneSlider = dashSkillRef.Timer.GetProgressAmount();
                chargingSlider.Peek().value = progressOneSlider;
            }));
        }
        
        private void IntervalEventHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {}));
        }
        
        private void FinishedEventHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {}));
        }
        
        // UseStack
        private void UseEventHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                if(chargingSlider.Count != 0) 
                    chargingSlider.Peek().value = 0;
                chargingSlider.Push(existsliders.Pop());
                chargingSlider.Peek().value = progressOneSlider;
                chargingSlider.Peek().image.color = _chargingColor;
            }));
        }
        
        // OnRecoverOnce
        private void RecoverEventHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                chargingSlider.Peek().value = 1f;
                existsliders.Push(chargingSlider.Pop());
                existsliders.Peek().image.color = _existColor;
            }));
        }

    }
}