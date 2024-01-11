using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_EO_Idle : Node
{
    Transform t;
    public BTT_EO_Idle(Transform transform)
    {
        t = transform;
    }

    public override NodeState Evaluate()
    {
        t.GetComponent<ElderOne>().IsLook = false;
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            t.GetComponent<ElderOne>().animator.SetBool("IsIdle3", false);
            t.GetComponent<ElderOne>().animator.SetBool("IsIdle2", true);
        }
        else
        {
            t.GetComponent<ElderOne>().animator.SetBool("IsIdle2", false);
            t.GetComponent<ElderOne>().animator.SetBool("IsIdle3", true);
        }

        return NodeState.Success;
    }
}
