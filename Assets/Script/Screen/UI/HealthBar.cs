using System.Collections;
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

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    private void OnDestroy() {
        if(LifeCompositeRef != null) {LifeCompositeRef.ClearEvents();}
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

    IEnumerator DoAndRenderUI(UnityAction action)
    {
        action.Invoke(); yield return new WaitForEndOfFrame();
    }

    public void TurnOffUI()
    {
        this.StopAllCoroutines();
        slider.gameObject.SetActive(false);
    }
}
