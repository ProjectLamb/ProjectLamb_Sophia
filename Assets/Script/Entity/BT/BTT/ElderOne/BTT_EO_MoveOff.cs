using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_MoveOff : Node
{
    Transform transform;
    public BTT_EO_MoveOff(Transform transform)
    {
        this.transform = transform;
        state = NodeState.Failure;
    }

    public override NodeState Evaluate()
    {
        if (state == NodeState.Success)
            return NodeState.Success;

        transform.GetComponent<Enemy>().animator.SetBool("IsWalk", false);
        transform.GetComponent<Enemy>().nav.enabled = false;

        return state = NodeState.Success;
    }
}
