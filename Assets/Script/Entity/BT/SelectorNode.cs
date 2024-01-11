using System.Collections;
using System.Collections.Generic;

public class SelectorNode : Node
{
    public SelectorNode() : base() { }

    public SelectorNode(List<Node> child) : base(child) { }

    public override NodeState Evaluate()
    {
        foreach(Node n in childNode)
        {
            switch(n.Evaluate())
            {
                case NodeState.Failure:
                    continue;
                case NodeState.Success:
                    return state = NodeState.Success;
                case NodeState.Running:
                    return state = NodeState.Running;
                default:
                    continue;
            }
        }

        return state = NodeState.Failure;
    }
}