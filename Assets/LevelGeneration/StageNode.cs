using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNode
{
    public Vector2 Position
    {
        get;
        set;
    }

    public StageNode Parent
    {
        get;
        set;
    }

    public List<StageNode> Children
    {
        get;
    }

    public Vector2 Delta
    {
        get;
        set;
    }

    public StageNode(Vector2 position, StageNode parent)
    {
        Position = position;
        Parent = parent;
        Children = new List<StageNode>();

        Delta = new Vector2(1, 0);

        if (Parent != null)
        {
            Parent.Children.Add(this);
            Delta = position - parent.Position;
        }
    }
}
