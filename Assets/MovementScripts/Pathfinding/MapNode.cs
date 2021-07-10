using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode 
{
    public bool Traversable;
    public MapNode(bool traversable)
    {
        Traversable = traversable;
    }
}

public enum TileType
{
    Water,
    Land 
}