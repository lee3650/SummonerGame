using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static int xSize = 36;
    public static int ySize = 17;

    static MapNode[,] Map = new MapNode[xSize, ySize]; //I believe all these entries are automatically null. 
    
    public static void SetMapSize(Vector2 mapSize)
    {
        xSize = (int)mapSize.x;
        ySize = (int)mapSize.y;
    }

    public static void SetMap(MapNode[,] newMap)
    {
        Map = newMap;
    }

    public static bool IsPointTraversable(Vector2 point)
    {
        return IsPointTraversable(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
    }
    
    public static void PrintMap()
    {
        string result = "";

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (IsPointTraversable(x, y))
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

    public static bool IsPointTraversable(int x, int y)
    {
        if (x > xSize || y > ySize || x < 0 || y < 0)
        {
            return false;
        }

        return Map[x, y].Traversable;
    }

    public static void WritePoint(int x, int y, MapNode newNode)
    {
        Map[x, y] = newNode;
    }
}
