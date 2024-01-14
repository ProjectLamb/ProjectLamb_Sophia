using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_ResetAnimParam : Node
{
    Transform transform;
    public BTT_EO_ResetAnimParam(Transform transform)
    {
        this.transform = transform;
    }

    public override NodeState Evaluate()
    {
        /*foreach (string b in transform.GetComponent<ElderOne>().boolArray)
            transform.GetComponent<Enemy>().animator.SetBool(b, false);*/
        transform.GetComponent<Enemy>().animator.SetBool("IsAttackWalk", false);
        foreach (string t in transform.GetComponent<ElderOne>().triggerArray)
            transform.GetComponent<Enemy>().animator.ResetTrigger(t);
        return NodeState.Success;
    }
}
