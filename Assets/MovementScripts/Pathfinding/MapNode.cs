using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode 
{
    public bool Traversable;
    public TileType TileType;
}

public enum TileType
{
    Water,
    Land 
}