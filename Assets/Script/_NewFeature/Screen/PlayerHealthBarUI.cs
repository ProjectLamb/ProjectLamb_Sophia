using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sophia.Composite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sophia.UserInterface
{

    public interface IHealthBarUI<T> {
        public void SetReferenceComposite(T Composite);
    }

    public class PlayerHealthBarUI : Michsky.UI.Shift.SliderManager, IHealthBarUI<LifeComposite> {
#region Inherited
//      [Header("Resources")]
//      public TextMeshProUGUI valueText;

//      [Header("Saving")]
//      public bool enableSaving = false;
//      public string sliderTag = "Tag Text";
//      public float defaultValue = 1;

//      [Header("Settings")]
//      public bool usePercent = false;
//      public bool showValue = true;
//      public bool useRoundValue = false;
        private StringBuilder stringBuilder = new();
        private Slider healthSlider;
        private Slider barrierSlider;

#endregion
        private LifeComposite lifeCompositeRef;
        public void SetReferenceComposite(LifeComposite lifeComposite) {
            lifeCompositeRef = lifeComposite;
            lifeCompositeRef.MaxHp.OnStatChanged.AddListener(MaxHpUpdatedHandler);
            lifeCompositeRef.OnHpUpdated        += HpUpdatedHandler;
            lifeCompositeRef.OnBarrier          += BarrierHandler;
            lifeCompositeRef.OnBarrierUpdated   += BarrierUpdatedHandler;
            lifeCompositeRef.OnBreakBarrier     += BreakBarrierHandler;
            lifeCompositeRef.OnDamaged          += DamagedHandler;
            lifeCompositeRef.OnHeal             += HealHandler;
            lifeCompositeRef.OnEnterDie         += EnterDieHandler;
            lifeCompositeRef.OnExitDie          += ExitDieHandler;
            lifeCompositeRef.OnExitDie          += ExitDieHandler;
            healthSlider.maxValue                 = lifeCompositeRef.MaxHp.GetValueByNature();
            healthSlider.value                    = lifeCompositeRef.CurrentHealth;
            barrierSlider                         = InGameScreenUI.Instance._playerBarrierBarUI; 
            barrierSlider.maxValue                = lifeCompositeRef.MaxHp.GetValueForce();
            barrierSlider.value                   = 0;


            stringBuilder.Append(healthSlider.value + " / " + healthSlider.maxValue);
            valueText.text = stringBuilder.ToString();
        }

        private void Awake() {
            healthSlider = GetComponent<Slider>();           
        }

        private void Start() {
            if (showValue == false)
                valueText.enabled = false;
        }
        private void Update() {}

        private void MaxHpUpdatedHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                healthSlider.maxValue                 = lifeCompositeRef.MaxHp.GetValueByNature();
                barrierSlider.maxValue                = lifeCompositeRef.MaxHp.GetValueByNature();
                stringBuilder.Append(lifeCompositeRef.CurrentHealth + lifeCompositeRef.CurrentBarrier + " / " + lifeCompositeRef.MaxHp.GetValueByNature());
                valueText.text = stringBuilder.ToString();
            }));
        }
        private void HpUpdatedHandler(float input) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                healthSlider.value                    = lifeCompositeRef.CurrentHealth;
                stringBuilder.Append(lifeCompositeRef.CurrentHealth + lifeCompositeRef.CurrentBarrier + " / " + lifeCompositeRef.MaxHp.GetValueByNature());
                valueText.text = stringBuilder.ToString();
            }));
        }

        private void BarrierHandler(float input) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                barrierSlider.value = lifeCompositeRef.CurrentBarrier;
                stringBuilder.Append(lifeCompositeRef.CurrentHealth + lifeCompositeRef.CurrentBarrier + " / " + lifeCompositeRef.MaxHp.GetValueByNature());
                valueText.text = stringBuilder.ToString();
            }));
        }

        private void BarrierUpdatedHandler(float input) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                barrierSlider.value = lifeCompositeRef.CurrentBarrier;
                stringBuilder.Append(lifeCompositeRef.CurrentHealth + lifeCompositeRef.CurrentBarrier + " / " + lifeCompositeRef.MaxHp.GetValueByNature());
                valueText.text = stringBuilder.ToString();
            }));
        }
        private void BreakBarrierHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                barrierSlider.value = 0;
                stringBuilder.Append(lifeCompositeRef.CurrentHealth + lifeCompositeRef.CurrentBarrier + " / " + lifeCompositeRef.MaxHp.GetValueByNature());
                valueText.text = stringBuilder.ToString();
            }));
        }
        private void DamagedHandler(DamageInfo damage) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {

            }));
        }
        private void HealHandler(float input) {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {
                stringBuilder.Clear();
                healthSlider.value                    = (int)lifeCompositeRef.CurrentHealth;
                stringBuilder.Append(healthSlider.value + " / " + healthSlider.maxValue);
                valueText.text = stringBuilder.ToString();
            }));
        }
        private void EnterDieHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {}));
        }
        private void ExitDieHandler() {
            StartCoroutine(AsyncRender.PerformAndRenderUI(() => {}));
        }

        private void OnDestroy() {
            lifeCompositeRef.ClearEvents();
            lifeCompositeRef.MaxHp.OnStatChanged.RemoveListener(MaxHpUpdatedHandler);
        }
    }
}