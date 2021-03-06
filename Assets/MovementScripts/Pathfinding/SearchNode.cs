using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode
{
    public SearchNode NextNode = null; 

    public SearchNode ParentNode = null; 

    public float g;
    public float h;
    public float f; 

    public int x, y;

    public Vector2Int Pos
    {
        get
        {
            return position;
        } 
        set
        {
            position = value;
            x = position.x;
            y = position.y; 
        }
    }

    private Vector2Int position;
    
    public SearchNode(Vector2Int pos, SearchNode parent)
    {
        Pos = pos;
        ParentNode = parent; 
    }

    public SearchNode(int x, int y)
    {
        this.x = x;
        this.y = y; 
    }

    public SearchNode(int x, int y, SearchNode parent)
    {
        this.x = x;
        this.y = y;
        ParentNode = parent; 
    }

    public SearchNode(float f)
    {
        this.f = f;
    }
}
