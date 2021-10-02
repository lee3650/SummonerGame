using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNode 
{
    public TileType TileType;
    private bool Traversable;
    public int TraversalCost = 1;
    private bool UseCanGoThroughWalls;

    public MapNode(bool traversable)
    {
        Traversable = traversable;
    }
    public MapNode(bool traversable, TileType tileType)
    {
        Traversable = traversable;
        TileType = tileType;
        TraversalCost = GetTraversalCost();
        UseCanGoThroughWalls = ShouldUseCanGoThroughWalls();
    }

    private bool ShouldUseCanGoThroughWalls()
    {
        switch (TileType)
        {
            case TileType.Barracks:
            case TileType.BreakableWall:
            case TileType.ArcherBarracks:
            case TileType.WallGenerator:
            case TileType.TrapGenerator:
            case TileType.Miner: 
                return true; 
        }
        return false; 
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
            case TileType.Barracks:
            case TileType.TrapGenerator:
            case TileType.Miner:
            case TileType.WallGenerator:
            case TileType.ArcherBarracks:
                return 1;
            case TileType.BreakableWall:
            case TileType.SummonTotem:
                return 15;
            case TileType.Gate:
                return 8;
            case TileType.OceanTransition:
            case TileType.Wall:
            case TileType.OceanTile:
            case TileType.IslandTile:
                return 100; 
        }
        throw new System.Exception("Could not get traversal cost for tile type " + TileType);
    }

    public bool IsTraversable(bool CanGoThroughWalls)
    {
        if (UseCanGoThroughWalls)
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
                return "Stone Tile. Increases generator capacity by 50% when a generator is placed on it."; //I actually don't know the value
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
            case TileType.ArcherBarracks:
                return "Archer barracks. Spawns archers that are positioned based on archer blueprints.";
            case TileType.WallGenerator:
                return "Wall Generator. Spawns walls based on wall and gate blueprints.";
            case TileType.SummonTotem:
                return "Summon Totem. Any blueprint can be placed next to it.";
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
    ArcherBarracks,
    WallGenerator, 
    SummonTotem,
    TrapGenerator, 
    Any, 
    BottomOfMap,
    TopOfMap,
    LeftOfMap,
    RightOfMap,
    OceanTransition,
    OceanTile, 
    IslandTile, 
}