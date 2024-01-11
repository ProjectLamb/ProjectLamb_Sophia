using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BTT_TurnToTarget : Node
{
    Transform transform;
    Transform target;
    public BTT_TurnToTarget(Transform transform, Transform target)
    {
        this.transform = transform;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        
        return NodeState.Success;
    }
}
