using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Feature_NewData;

public class HealthBarUI : MonoBehaviour
{
    public LifeComposite LifeCompositeRef;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    private void Start()
    {
        LifeCompositeRef ??= GetComponentInParent<ILifeAccessable>().GetLifeComposite();
        int intValue = LifeCompositeRef.MaxHp;
        slider.maxValue = (float)intValue;

        LifeCompositeRef.AddOnUpdateEvent(UpdateFillAmount)
                        .AddOnEnterDieEvent(TurnOffUI);

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    public void SetLifeComposite(LifeComposite LifeComposite)
    {
        int intValue = LifeComposite.MaxHp;
        slider.maxValue = (float)intValue;

        LifeCompositeRef = LifeComposite;
        LifeCompositeRef.AddOnUpdateEvent(UpdateFillAmount)
                        .AddOnEnterDieEvent(TurnOffUI);

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    public void DrawForce()
    {
        StartCoroutine(DoAndRenderUI(() =>
        {
            slider.value = LifeCompositeRef.CurrentHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }));
    }

    IEnumerator DoAndRenderUI(UnityAction action)
    {
        action.Invoke(); yield return new WaitForEndOfFrame();
    }

    private void UpdateFillAmount(float currentHp)
    {
        // Debug.Log(currentHp);
        StartCoroutine(DoAndRenderUI(() =>
        {
            slider.value = currentHp;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }));
    }

    public void TurnOffUI()
    {
        this.StopAllCoroutines();
        slider.gameObject.SetActive(false);
    }
}