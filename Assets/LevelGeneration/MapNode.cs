using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode 
{
    public TileType TileType;
    public bool Traversable;
    public MapNode(bool traversable)
    {
        Traversable = traversable;
    }
    public MapNode(bool traversable, TileType tileType)
    {
        Traversable = traversable;
        TileType = tileType;
    }
}

public enum TileType
{
    Water,
    Land,
    Valley,
    Bridge, 
    Wall, 
}