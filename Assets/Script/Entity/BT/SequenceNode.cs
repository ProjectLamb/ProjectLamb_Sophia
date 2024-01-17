using System.Collections;
using System.Collections.Generic;

public class SequenceNode : Node
{
    public SequenceNode() : base() { }

    public SequenceNode(List<Node> child) : base(child) { }

    public override NodeState Evaluate()
    {
        if (childNode.Count == 0)
            return state = NodeState.Failure;

        int successCount = 0;
        //bool IsNowRunning = false;
        foreach(Node n in childNode)
        {
            switch(n.Evaluate())
            {
                case NodeState.Failure:
                    return state = NodeState.Failure;
                case NodeState.Success:
                    successCount++;
                    continue;
                case NodeState.Running:
                    return state = NodeState.Running;
                    /*IsNowRunning = true;
                    continue;*/
                default:
                    continue;
            }
        }

        if (successCount == childNode.Count)
        {
            foreach (Node n in childNode)
            {
                n.ResetState();
            }
            return state = NodeState.Success;
        }
        return state = NodeState.Running;
        //return state = IsNowRunning ? NodeState.Running : NodeState.Success;
    }
}