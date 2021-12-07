using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandNode
{
    public Vector2Int Position;
    public int Size;

    public List<IslandNode> Next;

    public IslandNode Parent;

    public IslandNode(Vector2Int pos, int size, IslandNode parent)
    {
        Size = size;
        Position = pos;
        Next = new List<IslandNode>();
        Parent = parent; 
    }
}
