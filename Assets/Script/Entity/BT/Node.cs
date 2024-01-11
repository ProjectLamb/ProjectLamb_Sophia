using System.Collections;
using System.Collections.Generic;

public enum NodeState
{
    Success,
    Failure,
    Running,
}

public abstract class Node
{
    protected NodeState state;
    public Node parentNode;
    protected List<Node> childNode = new List<Node>();

    public Node()
    {
        parentNode = null;
    }

    public Node(List<Node> child)
    {
        foreach(var c in child)
        {
            AddChildNode(c);
        }
    }

    public void AddChildNode(Node child)
    {
        childNode.Add(child);
        child.parentNode = this;
    }

    public void ResetState()
    {
        state = NodeState.Failure;
    }

    public abstract NodeState Evaluate();
}
