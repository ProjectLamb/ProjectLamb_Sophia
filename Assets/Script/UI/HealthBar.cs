using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sandbag sandbag;
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    private void Awake() {
        fill.color = gradient.Evaluate(1f);
    }

    public void SetSlider(){
        slider.value = (((float)sandbag.CurHP / (float)sandbag.MaxHP) * slider.maxValue);
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //slider.value = ((float)sandbag.sandbagData.CurHP / sandbag.sandbagData.MaxHP) * slider.maxValue;
    }
}
