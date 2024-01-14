using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_AttackWalkEnd : Node
{
    Transform transform;
    public BTT_EO_AttackWalkEnd(Transform transform)
    {
        this.transform = transform;
        state = NodeState.Failure;
    }

    public override NodeState Evaluate()
    {
        if(state == NodeState.Failure)
        {
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackCharge");
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackWalk");

            transform.GetComponent<BehaviorTree>().blackBoard.floatDict["Distance"] = transform.GetComponent<ElderOne>().attackRange;
            transform.GetComponent<BehaviorTree>().blackBoard.boolDict["PhaseSkill"] = true;
        }

        if (transform.GetComponent<Enemy>().animator.GetBool("IsAttackWalkEnd"))
            return state = NodeState.Running;
        else
        {
            transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] = 0;
            return state = NodeState.Success;
        }
    }
}
