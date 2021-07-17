using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode 
{
    public TileType TileType;
    public bool Traversable;
    public int TraversalCost = 1; 
    public MapNode(bool traversable)
    {
        Traversable = traversable;
    }
    public MapNode(bool traversable, TileType tileType)
    {
        Traversable = traversable;
        TileType = tileType;
        TraversalCost = GetTraversalCost(); 
    }

    private int GetTraversalCost()
    {
        switch (TileType)
        {
            case TileType.Bridge:
            case TileType.Valley:
            case TileType.Water:
            case TileType.Land:
                return 1; 
            case TileType.BreakableWall:
                return 20;
            case TileType.Gate:
                return 3; 
            case TileType.Wall:
                return 100; 
        }
        throw new System.Exception("Could not get traversal cost for tile type " + TileType);
    }    
}

public enum TileType
{
    Water,
    Land,
    Valley,
    Bridge, 
    Wall, 
    BreakableWall, 
    Gate, 
}