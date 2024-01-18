using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sophia_Carriers;

public class Boss : Enemy
{
    protected BehaviorTree behaviorTree;
    protected FieldOfView fov;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<BehaviorTree>(out behaviorTree);
        TryGetComponent<FieldOfView>(out fov);
    }
    void Start()
    {
        
    }
}
