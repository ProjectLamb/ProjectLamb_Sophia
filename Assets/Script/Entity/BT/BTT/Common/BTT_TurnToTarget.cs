using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BTT_TurnToTarget : Node
{
    Transform transform;
    Transform target;
    float duration;
    public BTT_TurnToTarget(Transform transform, Transform target, float duration)
    {
        state = NodeState.Failure;
        this.transform = transform;
        this.target = target;
        this.duration = duration;
    }

    public override NodeState Evaluate()
    {
        if (state == NodeState.Success)
            return NodeState.Success;

        transform.DOLookAt(target.position, duration);

        return state = NodeState.Success;
    }
}
