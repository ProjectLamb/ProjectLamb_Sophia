using System.Collections;
using System.Collections.Generic;

public class SequenceNode : Node
{
    public SequenceNode() : base() { }

    public SequenceNode(List<Node> child) : base(child) { }

    public override NodeState Evaluate()
    {
        bool IsNowRunning = false;
        foreach(Node n in childNode)
        {
            switch(n.Evaluate())
            {
                case NodeState.Failure:
                    return state = NodeState.Failure;
                case NodeState.Success:
                    continue;
                case NodeState.Running:
                    IsNowRunning = true;
                    continue;
                default:
                    continue;
            }
        }

        return state = IsNowRunning ? NodeState.Running : NodeState.Success;
    }
}