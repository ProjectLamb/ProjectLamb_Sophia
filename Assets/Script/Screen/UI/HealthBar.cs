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
        LifeCompositeRef ??= GetComponentInParent<Entity>().Life;
        int intValue = LifeCompositeRef.MaxHp;
        slider.maxValue = (float)intValue;

        LifeCompositeRef.OnHpUpdated += UpdateFillAmount;
        LifeCompositeRef.OnEnterDie += TurnOffUI;

        StartCoroutine(DoAndRenderUI(() => { fill.color = gradient.Evaluate(1f); }));
    }

    private void OnDestroy() {
        LifeCompositeRef.OnHpUpdated -= UpdateFillAmount;
        LifeCompositeRef.OnEnterDie  -= TurnOffUI;
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
