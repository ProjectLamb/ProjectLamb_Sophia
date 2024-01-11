using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_DistanceToTarget : Node
{
    Transform transform;
    Transform target;
    float distance;

    public BTT_DistanceToTarget(Transform transform, Transform target, string key)
    {
        this.transform = transform;
        this.target = target;
        distance = transform.GetComponent<BehaviorTree>().blackBoard.floatDict[key];
    }

    public override NodeState Evaluate()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= distance)
            return NodeState.Success;
        else
            return NodeState.Failure;
    }
}
