using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sophia;

public class HitCanvasShadeScript : MonoBehaviour
{
    public bool IsRepeating = false;
    Image canvasImage;
    Sequence sequence;
    private void Awake()
    {
        TryGetComponent<Image>(out canvasImage);
        sequence = DOTween.Sequence();
        sequence.Append(canvasImage.DOColor(new Color(1, 0, 0, 0.03f), 0));
        sequence.Append(canvasImage.DOColor(new Color(1, 0, 0, 0.12f), 1f));
        sequence.Append(canvasImage.DOColor(new Color(1, 0, 0, 0.03f), 1f));
        sequence.Pause();
        canvasImage.enabled = false;
    }

    public void Invoke(DamageInfo damageInfo)
    {
        canvasImage.enabled = true;
        if (damageInfo.damageHandleType == DamageHandleType.BarrierCoved || damageInfo.damageHandleType == DamageHandleType.Dodge) return;
        canvasImage.color = new Color(1, 0, 0, 0.5f);
        canvasImage.DOColor(new Color(0, 0, 0, 0), 0.075f * damageInfo.damageAmount).OnComplete(() => {canvasImage.enabled = false;});
    }

    public void Repeat()
    {
        canvasImage.enabled = true;
        sequence.SetLoops(-1);
        sequence.Restart();
        IsRepeating = true;
    }

    public void UnRepeat()
    {
        canvasImage.enabled = false;
        sequence.Pause();
        IsRepeating = false;
    }
}
