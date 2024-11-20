using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class BossPattern : MonoBehaviour
{
    [SerializeField] DecalProjector indicator;
    [SerializeField] GameObject projectile;

    public bool IsPreview;

    public float IndicatorDisplayTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        if (IsPreview)
        {
            return;
        }

        DisplayIndicator();
        StartCoroutine(DelayProjectile());
    }

    [ContextMenu("Display Preview Indicator")]
    public void DisplayPreviewIndicator(float width, float height, float duration)
    {
        indicator.enabled = true;

        indicator.size = new Vector3(0, 0, 0);
        DOTween.To(() => indicator.size, x => indicator.size = x, new Vector3(width, height, height), duration).OnComplete(()=>{
            StartCoroutine(DelayDestroy(2f));
        });
    }

    IEnumerator DelayDestroy(float sec)
    {
        yield return new WaitForSeconds(sec);

        indicator.enabled = false;
        Destroy(gameObject);
    }

    void DisplayIndicator()
    {
        indicator.enabled = true;
    }

    IEnumerator DelayProjectile()
    {
        yield return new WaitForSeconds(IndicatorDisplayTime);

        InstantiateProjectile();
    }

    void InstantiateProjectile()
    {

    }
}
