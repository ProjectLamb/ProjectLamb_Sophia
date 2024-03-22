using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using Sophia.Instantiates;

namespace Sophia.UserInterface
{
    public class PlayerSkillCoolUI : MonoBehaviour
    {
        public UnityEngine.UI.Image     fill;
        public UnityEngine.UI.Image     icon;
        public TextMeshProUGUI          textMeshPro;

        public Sprite defaultSprite;

        private Sophia.Composite.CoolTimeComposite TimerRef;

        public void SetSkill(Skill skill)
        {
            TimerRef = skill.GetCoolTimeComposite();

            TimerRef.AddOnTickingEvent(UpdateFillAmount)
                    .AddOnUseEvent(UseStack)
                    .AddOnFinishedEvent(RecoverStack)
                    .AddOnInitialized(ResetUI);

            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { 
                fill.fillAmount = 0;
                textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); 
                icon.sprite = skill.GetSprite();
            }));
        }

        public void RemoveSkill() {
            TimerRef = null;
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { 
                fill.fillAmount = 1;
                textMeshPro.text = "";
                icon.sprite = defaultSprite;
            }));
        }

        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - TimerRef.GetProgressAmount();
        }

        public void ResetUI()
        {
            fill.fillAmount = 0;
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.BaseStacksCount.ToString(); }));
        }

        public void DrawForce()
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.BaseStacksCount.ToString(); }));
        }

        private void UseStack()
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); }));
        }
        private void RecoverStack()
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => { textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); }));
        }

        private void OnDestroy() {
            TimerRef.RemoveOnTickingEvent(UpdateFillAmount)
                    .RemoveOnUseEvent(UseStack)
                    .RemoveOnFinishedEvent(RecoverStack)
                    .RemoveOnInitialized(ResetUI);
        }
    }
}