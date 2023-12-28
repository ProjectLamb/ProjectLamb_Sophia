using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Feature_NewData;

public class HealthBarUI : MonoBehaviour
{
    public LifeManager lifeManagerRef;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    private void Start()
    {
        lifeManagerRef ??= GetComponentInParent<ILifeAccessable>().GetLifeManager();
        int intValue = lifeManagerRef.MaxHp;
        slider.maxValue = (float)intValue;

        lifeManagerRef.AddOnUpdateEvent(UpdateFillAmount)
                        .AddOnEnterDieEvent(TurnOffUI);

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    public void SetLifeManager(LifeManager lifeManager)
    {
        int intValue = lifeManager.MaxHp;
        slider.maxValue = (float)intValue;

        lifeManagerRef = lifeManager;
        lifeManagerRef.AddOnUpdateEvent(UpdateFillAmount)
                        .AddOnEnterDieEvent(TurnOffUI);

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    public void DrawForce()
    {
        StartCoroutine(DoAndRenderUI(() =>
        {
            slider.value = lifeManagerRef.CurrentHealth;
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