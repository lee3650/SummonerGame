using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                map[x, y] = new MapNode(true, TileType.Land);
            }
        }
    }
}
