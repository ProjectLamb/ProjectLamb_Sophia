using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_UpperCut : Node
{
    Transform transform;

    public BTT_EO_UpperCut(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackLeft");
        transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackRight");
        transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackBoth");

        transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] = 0;
        transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackUpperCut");
        return state = NodeState.Success;
    }
}
