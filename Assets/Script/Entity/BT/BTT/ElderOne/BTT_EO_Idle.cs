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
            t.GetComponent<ElderOne>().animator.ResetTrigger("DoIdle3");
            t.GetComponent<ElderOne>().animator.SetTrigger("DoIdle2");
        }
        else
        {
            t.GetComponent<ElderOne>().animator.ResetTrigger("DoIdle2");
            t.GetComponent<ElderOne>().animator.SetTrigger("DoIdle3");
        }

        return NodeState.Success;
    }
}
