using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static int xSize = 36;
    public static int ySize = 17;

    static MapNode[,] Map = new MapNode[xSize, ySize]; //I believe all these entries are automatically null. 
    
    public static void InitMap()
    {
        Map = new MapNode[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Map[x, y] = new MapNode(false, TileType.Wall);
            }
        }
    }

    public static void SetMapSize(Vector2 mapSize)
    {
        xSize = (int)mapSize.x;
        ySize = (int)mapSize.y;
    }

    public static bool IsPointTraversable(Vector2 point, bool CanGoThroughWalls)
    {
        return IsPointTraversable(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), CanGoThroughWalls);
    }

    public static MapNode ReadPoint(int x, int y)
    {
        return Map[x, y];
    }

    public static Vector2 GetClosestValidTile(Vector2 start)
    {
        start = VectorRounder.RoundVector(start);

        float minDistance = Mathf.Infinity;
        Vector2 result = start; 

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (IsPointTraversable(x, y, false))
                {
                    if (Vector2.Distance(new Vector2(x, y), start) < minDistance)
                    {
                        minDistance = Vector2.Distance(new Vector2(x, y), start);
                        result = new Vector2(x, y);
                    }
                }
            }
        }

        return result;
    }

    public static int GetTraversalCost(int x, int y)
    {
        return Map[x, y].TraversalCost;
    }

    public static void PrintMap()
    {
        string result = "";

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (IsPointTraversable(x, y, false))
                {
                    result += "0";
                } else
                {
                    result += "1";
                }
            }
            result += "\n";
        }

        MonoBehaviour.print(result);
    }

    public static bool IsPointTraversable(int x, int y, bool CanGoThroughWalls)
    {
        if (x >= xSize || y >= ySize || x < 0 || y < 0)
        {
            return false;
        }

        return Map[x, y].IsTraversable(CanGoThroughWalls);
    }

    public static void WritePoint(int x, int y, MapNode newNode)
    {
        Map[x, y] = newNode;
    }

    public static MapNode[,] GetMap()
    {
        return Map;
    }
}
