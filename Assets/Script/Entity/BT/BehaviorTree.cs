using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    public BlackBoard blackBoard;
    private Node rootNode;
    // Start is called before the first frame update
    protected void Start()
    {
        blackBoard = SetupBlackBoard();
        rootNode = SetupBehaviorTree();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (rootNode == null)
            return;
        rootNode.Evaluate();

    }

    protected abstract Node SetupBehaviorTree();
    protected abstract BlackBoard SetupBlackBoard();
}
