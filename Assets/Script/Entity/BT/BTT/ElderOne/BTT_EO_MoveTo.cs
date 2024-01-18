using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_MoveTo : Node
{
    Transform transform;
    UnityEngine.AI.NavMeshAgent nav;
    Transform target;
    public BTT_EO_MoveTo(Transform transform, Transform target)
    {
        this.transform = transform;
        nav = transform.GetComponent<Enemy>().nav;
        this.target = target;
        state = NodeState.Failure;
    }

    public override NodeState Evaluate()
    {
        transform.GetComponent<Enemy>().nav.enabled = true;
        nav.SetDestination(target.position);

        if (transform.GetComponent<Enemy>().animator.GetBool("IsAttackWalk"))
            return state = NodeState.Running;
        else
            return state = NodeState.Success;
    }
}
