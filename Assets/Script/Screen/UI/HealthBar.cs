using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Entity       ownerEntity;
    public Slider       slider;
    public Image        fill;
    public Gradient     gradient;

    private void Awake() {
        ownerEntity ??= GetComponentInParent<Entity>();
        fill.color  = gradient.Evaluate(1f);
    }
    
    private void Update() {
        SetSlider();
    }

    public void SetSlider(){
        slider.value = (((float)ownerEntity.CurrentHealth / (float)ownerEntity.GetFinalData().MaxHP) * slider.maxValue);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
