using System.Collections;
using Sophia.UserInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sophia.Composite.LifeComposite LifeCompositeRef;
    public Slider       slider;
    public Image        fill;
    public Gradient     gradient;

    private void Start() {
        LifeCompositeRef ??= GetComponentInParent<Sophia.Entitys.Entity>().GetLifeComposite();
        int intValue = LifeCompositeRef.MaxHp;
        slider.maxValue = (float)intValue;

        LifeCompositeRef.OnHpUpdated += UpdateFillAmount;
        LifeCompositeRef.OnEnterDie += TurnOffUI;

        StartCoroutine(AsyncRender.PerformAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    private void OnDestroy() {
        if(LifeCompositeRef != null) {LifeCompositeRef.ClearEvents();}
    }

    private void UpdateFillAmount(float currentHp)
    {
        // Debug.Log(currentHp);
        StartCoroutine(AsyncRender.PerformAndRenderUI(() =>
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
