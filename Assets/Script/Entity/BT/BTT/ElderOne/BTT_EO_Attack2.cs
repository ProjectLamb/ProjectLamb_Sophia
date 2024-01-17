using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_Attack2 : Node
{
    Transform transform;
    public BTT_EO_Attack2(Transform transform)
    {
        state = NodeState.Failure;
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        if (state == NodeState.Failure)
        {
            transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackBoth2");
            transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] += 1;
            //Debug.Log(transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"]);
        }

        if (transform.GetComponent<Enemy>().animator.GetBool("IsAttack"))
            return state = NodeState.Running;
        else
        {
            return state = NodeState.Success;
        }
    }
}
