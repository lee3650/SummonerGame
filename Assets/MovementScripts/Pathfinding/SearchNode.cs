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
}
