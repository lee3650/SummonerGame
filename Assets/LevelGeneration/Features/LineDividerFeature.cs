using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDividerFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        AddLine(ySize / 2, xSize, map);
    }

    protected void AddLine(int y, int xSize, MapNode[,] map)
    {
        for (int x = 2; x < xSize - 2; x++)
        {
            map[x, y] = new MapNode(false, TileType.Valley);
        }
    }
}
