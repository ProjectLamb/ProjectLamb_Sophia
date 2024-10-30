using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sophia.UserInterface
{

    public class HealthBarBoUI : MonoBehaviour
    {
        public Sophia.Composite.LifeComposite LifeCompositeRef;
        public Slider slider;
        public Image fill;
        public Gradient gradient;

        private void Start()
        {
            LifeCompositeRef ??= FindFirstObjectByType<Sophia.Entitys.ElderOne>().GetLifeComposite();
            LifeCompositeRef.OnHpUpdated += UpdateFillAmount;
            LifeCompositeRef.OnEnterDie += TurnOffUI;

            StartCoroutine(
                GlobalAsync.PerformAndRenderUI(() => { 
                    int intValue = LifeCompositeRef.MaxHp;
                    slider.maxValue = (float)intValue;
                    slider.value = (float)intValue;
                    fill.color = gradient.Evaluate(1f); 
                })
            );
        }

        private void OnDestroy()
        {
            if(LifeCompositeRef != null) {
                LifeCompositeRef.OnHpUpdated -= UpdateFillAmount;
                LifeCompositeRef.OnEnterDie -= TurnOffUI;
            }
        }

        public void DrawForce()
        {
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() =>
            {
                slider.value = LifeCompositeRef.CurrentHealth;
                fill.color = gradient.Evaluate(slider.normalizedValue);
            }));
        }

        private void UpdateFillAmount(float currentHp)
        {
            // Debug.Log(currentHp);
            StartCoroutine(GlobalAsync.PerformAndRenderUI(() =>
            {
                slider.value = currentHp;
                fill.color = gradient.Evaluate(slider.normalizedValue);
            }));
        }

        public void TurnOffUI()
        {
            this.StopAllCoroutines();
            this.gameObject.SetActive(false);
            //slider.gameObject.SetActive(false);
        }
    }

}