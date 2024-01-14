using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_Attack : Node
{
    Transform transform;
    public BTT_EO_Attack(Transform transform)
    {
        state = NodeState.Failure;
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        if (state == NodeState.Failure)
        {
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackLeft");
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackRight");
            transform.GetComponent<Enemy>().animator.ResetTrigger("DoAttackBoth");
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackLeft");
                    //transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] += 1;
                    break;
                case 1:
                    transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackRight");
                    //transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] += 1;
                    break;
                case 2:
                    transform.GetComponent<Enemy>().animator.SetTrigger("DoAttackBoth");
                    //transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] += 2;
                    break;
            }
            //Debug.Log(transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"]);
            transform.GetComponent<BehaviorTree>().blackBoard.intDict["AttackCount"] += 1;
            state = NodeState.Running;
        }
        
        if (transform.GetComponent<Enemy>().animator.GetBool("IsAttack"))
            return state = NodeState.Running;
        else
        {
            return state = NodeState.Success;
        }
    }
}
