using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_CheckBool : Node
{
    Transform transform;
    string k;
    public BTT_CheckBool(Transform transform, string key)
    {
        this.transform = transform;
        k = key;
    }

    public override NodeState Evaluate()
    {
        if (transform.GetComponent<BehaviorTree>().blackBoard.boolDict[k] == true)
            return state = NodeState.Success;
        else
            return state = NodeState.Failure;
    }
}
