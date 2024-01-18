using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTT_MoveTo : Node
{
    Transform transform;
    NavMeshAgent nav;
    Transform target;
    public BTT_MoveTo(Transform transform, Transform target)
    {
        this.transform = transform;
        nav = transform.GetComponent<Enemy>().nav;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        transform.GetComponent<Enemy>().animator.SetBool("IsWalk", true);
        transform.GetComponent<Enemy>().nav.enabled = true;
        nav.SetDestination(target.position);

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
            {
                return NodeState.Success;
            }
        }
        //return NodeState.Running;
        return NodeState.Success;
    }
}