using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossPattern : MonoBehaviour
{
    [SerializeField] DecalProjector indicator;
    [SerializeField] GameObject projectile;

    public float IndicatorDisplayTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        DisplayIndicator();
        StartCoroutine(DelayProjectile());
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
