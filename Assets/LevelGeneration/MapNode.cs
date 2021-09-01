using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode 
{
    public TileType TileType;
    private bool Traversable;
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
            case TileType.Gold:
            case TileType.Copper:
            case TileType.Silver:
            case TileType.Ore:
            case TileType.Hills:
            case TileType.Stone:
            case TileType.Marsh:
                return 1;
            case TileType.Barracks:
            case TileType.Miner:
            case TileType.BreakableWall:
                return 30;
            case TileType.Gate:
                return 10;
            case TileType.Wall:
                return 100; 
        }
        throw new System.Exception("Could not get traversal cost for tile type " + TileType);
    }

    public bool IsTraversable(bool CanGoThroughWalls)
    {
        if (TileType == TileType.BreakableWall)
        {
            return CanGoThroughWalls; 
        }
        return Traversable;
    }
}

public class TileDescription
{
    public static string GetTileDescription(TileType type)
    {
        string result = GetUnformattedTileDescription(type);
        return StringWidthFormatter.FormatStringToWidth(result, StringWidthFormatter.StandardWidth);
    }

    //hm. So, I'm making it harder to change any of these values now, because I have to update it in two places. 
    static string GetUnformattedTileDescription(TileType type)
    {
        switch (type)
        {
            case TileType.Copper:
                return "Copper Tile. Earns an income of 5 mana per turn if a miner is placed adjacent.";
            case TileType.Gold:
                return "Gold Tile. Earns an income of 10 mana per turn if a miner is placed adjacent.";
            case TileType.Silver:
                return "Silver Tile. Earns an income of 7.5 mana per turn if a miner is placed adjacent.";
            case TileType.Stone:
                return "Stone Tile. Reduces damage taken when a unit is standing on it by 25%."; //I actually don't know the value
            case TileType.Valley:
                return "Valley Tile";
            case TileType.Wall:
                return "Wall Tile";
            case TileType.Barracks:
                return "Barracks";
            case TileType.BreakableWall:
                return "Player Wall Tile";
            case TileType.Bridge:
                return "Bridge Tile";
            case TileType.Hills:
                return "Hill Tile. Increases damage dealt by 25% when a unit is standing on it.";
            case TileType.Water:
                return "Water Tile. Increases damage taken by 100% when a unit is standing on it."; //yeah in retrospect that seems a bit OP
            case TileType.Land:
                return "Land Tile";
        }

        return type.ToString();
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
    Ore,
    Copper,
    Silver,
    Gold,
    Miner,
    Marsh,
    Stone,
    Hills,
    Barracks,
}