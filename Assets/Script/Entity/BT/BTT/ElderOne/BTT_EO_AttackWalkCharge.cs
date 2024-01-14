using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_AttackWalkCharge : Node
{
    Transform transform;
    public BTT_EO_AttackWalkCharge(Transform transform)
    {
        state = NodeState.Failure;
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        if(state == NodeState.Failure)
        {
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackBoth2");
            transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackWalkCharge");
        }

        return state = NodeState.Success;
    }
}
