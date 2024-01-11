using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_MoveOff : Node
{
    Transform transform;
    public BTT_EO_MoveOff(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        transform.GetComponent<Enemy>().animator.SetBool("IsWalk", false);
        transform.GetComponent<Enemy>().nav.enabled = false;

        return NodeState.Success;
    }
}
