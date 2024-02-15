using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class Boss : Enemy
{
    protected FieldOfView fov;
    protected int phase = 1;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<FieldOfView>(out fov);
    }
    void Start()
    {
        
    }
}
