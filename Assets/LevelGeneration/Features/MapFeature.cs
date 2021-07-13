using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapFeature
{
    public abstract void AddFeature(int xSize, int ySize, MapNode[,] map); //x and y size is map size, not feature size 

    public static bool IsPointOnMap(Vector2 point, int xSize, int ySize)
    {
        return point.x >= 0 && point.y < xSize && point.y >= 0 && point.y < ySize;
    }
}