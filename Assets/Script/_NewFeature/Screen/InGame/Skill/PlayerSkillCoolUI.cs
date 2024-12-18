using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Sophia.Instantiates;

namespace Sophia.UserInterface
{
    public class PlayerSkillCoolUI : MonoBehaviour
    {
        public UnityEngine.UI.Image     fill;
        public UnityEngine.UI.Image     icon;
        public TextMeshProUGUI          textMeshPro;
        public GameObject key;
        public bool                     isActive = true;

        public Sprite defaultSprite;

        private Sophia.Composite.CoolTimeComposite TimerRef;

        private void OnDestroy() {
            TimerRef.RemoveOnTickingEvent(UpdateFillAmount)
                    .RemoveOnUseEvent(UseStack)
                    .RemoveOnFinishedEvent(RecoverStack)
                    .RemoveOnInitialized(ResetUI);
            TimerRef = null;
        }
        
        Queue<(Skill, UnityAction<Skill>)> _actionSkillQueue = new Queue<(Skill, UnityAction<Skill>)>();
        Queue<UnityAction> _actionVoidQueue = new Queue<UnityAction>();

        private void OnEnable()
        {
            while (_actionSkillQueue.Count > 0)
            {
                (Skill, UnityAction<Skill>)
                    frontData = _actionSkillQueue.Dequeue();
                StartCoroutine(GlobalAsync.PerformAndRenderUI(() => frontData.Item2.Invoke(frontData.Item1)));
            }
            while(_actionVoidQueue.Count > 0)
            {
                StartCoroutine(GlobalAsync.PerformAndRenderUI(_actionVoidQueue.Dequeue()));
            }
        }

        public void SetUIActivate()
        {
            isActive = true;
            while (_actionSkillQueue.Count > 0)
            {
                (Skill, UnityAction<Skill>)
                    frontData = _actionSkillQueue.Dequeue();
                StartCoroutine(GlobalAsync.PerformAndRenderUI(() => frontData.Item2.Invoke(frontData.Item1)));
            }
            while(_actionVoidQueue.Count > 0)
            {
                StartCoroutine(GlobalAsync.PerformAndRenderUI(_actionVoidQueue.Dequeue()));
            }
        }
        
        public void SetUIDeactivate()
        {
            isActive = false;
        }

        public void SetSkill(Skill skill)
        {
            TimerRef = skill.GetCoolTimeComposite();

            TimerRef.AddOnTickingEvent(UpdateFillAmount)
                    .AddOnUseEvent(UseStack)
                    .AddOnFinishedEvent(RecoverStack)
                    .AddOnInitialized(ResetUI);

            if (isActive == false)
            {
                _actionSkillQueue.Enqueue((skill, SkillSetUpdateLambda)); return;
            }
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() => SkillSetUpdateLambda(skill)));
        }

        public void SkillSetUpdateLambda(Skill skill)
        {
            fill.fillAmount = 0;
            textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString(); 
            icon.sprite = skill.GetSprite();
        }

        public void RemoveSkill() {
            if(TimerRef!=null) {
                TimerRef.RemoveOnTickingEvent(UpdateFillAmount)
                        .RemoveOnUseEvent(UseStack)
                        .RemoveOnFinishedEvent(RecoverStack)
                        .RemoveOnInitialized(ResetUI);
            }
                    
            TimerRef = null;
            if(isActive == false) { _actionVoidQueue.Enqueue(SkillRemoveUpdateLambda); return; }
            StartCoroutine(GlobalAsync.PerformAndRenderUI(SkillRemoveUpdateLambda));
        }
        
        public void SkillRemoveUpdateLambda()
        {
            fill.fillAmount = 1;
            textMeshPro.text = "";
            icon.sprite = defaultSprite;
        }

        private void UpdateFillAmount(float NoneUse)
        {
            fill.fillAmount = 1f - TimerRef.GetProgressAmount();
        }

        public void ResetUI()
        {
            fill.fillAmount = 0;
            if(isActive == false) { _actionVoidQueue.Enqueue(CoolTimeResetLambda); return; }
            StartCoroutine(GlobalAsync.PerformAndRenderUI(CoolTimeResetLambda));
        }

        public void CoolTimeResetLambda()
        {
            textMeshPro.text = TimerRef.stackCounter.BaseStacksCount.ToString();
        }

        public void DrawForce()
        {
            if(isActive == false) {_actionVoidQueue.Enqueue(DrawForceLambda); return;}
            StartCoroutine(GlobalAsync.PerformAndRenderUI(DrawForceLambda));
        }

        public void DrawForceLambda()
        {
            fill.fillAmount = 1f - TimerRef.GetProgressAmount();
            textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString();
        }

        private void UseStack()
        {
            if(isActive == false) {_actionVoidQueue.Enqueue(StackCounterUpdateLambda); return;}
            StartCoroutine(GlobalAsync.PerformAndRenderUI(StackCounterUpdateLambda));
        }
        private void RecoverStack()
        {
            if(isActive == false) {_actionVoidQueue.Enqueue(StackCounterUpdateLambda); return;}
            StartCoroutine(GlobalAsync.PerformAndRenderUI(StackCounterUpdateLambda));
        }

        public void StackCounterUpdateLambda() => textMeshPro.text = TimerRef.stackCounter.CurrentStacksCount.ToString();
    }
}