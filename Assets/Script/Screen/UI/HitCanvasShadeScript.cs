using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sophia;

public class HitCanvasShadeScript : MonoBehaviour
{
    Image canvasImage;
    private void Awake() {
        TryGetComponent<Image>(out canvasImage);
    }

    public void Invoke(DamageInfo damageInfo) {
        if(damageInfo.damageHandleType == DamageHandleType.BarrierCoved || damageInfo.damageHandleType == DamageHandleType.Dodge) return;
        canvasImage.color = new Color(1,0,0, 0.5f);
        canvasImage.DOColor(new Color(0,0,0, 0), 0.075f * damageInfo.damageAmount);
    }
}
