using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTT_CheckInt : Node
{
    Transform transform;
    string k;
    int value;
    public BTT_CheckInt(Transform transform, string key, int value)
    {
        this.transform = transform;
        k = key;
        this.value = value;
    }

    public override NodeState Evaluate()
    {
        if (transform.GetComponent<BehaviorTree>().blackBoard.intDict[k] >= value)
            return state = NodeState.Success;
        else
            return state = NodeState.Failure;
    }
}
