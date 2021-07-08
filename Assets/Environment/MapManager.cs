using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    const int xSize = 35;
    const int ySize = 16;

    static MapNode[,] Map = new MapNode[xSize, ySize]; //I believe all these entries are automatically null. 

    public static bool IsPointTraversable(Vector2 point)
    {
        if (Mathf.Round(point.x) > xSize || Mathf.Round(point.y) > ySize || point.x < 0 || point.y < 0)
        {
            return false; 
        }

        return Map[Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y)].Traversable;
    }
}
