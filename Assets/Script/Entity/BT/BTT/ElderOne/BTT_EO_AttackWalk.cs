using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_AttackWalk : Node
{
    Transform transform;
    public BTT_EO_AttackWalk(Transform transform)
    {
        state = NodeState.Failure;
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        if (state == NodeState.Success)
            return NodeState.Success;
        transform.GetComponent<BehaviorTree>().blackBoard.floatDict["Distance"] *= 5;
        if (transform.GetComponent<Enemy>().animator.GetBool("IsAttackWalk"))
            return state = NodeState.Running;
        else
        {
            return state = NodeState.Success;
        }
    }
}
