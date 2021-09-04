using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividerFeature : MapFeature
{
    //oh, so this takes x and y size... is it possible we planned for that to change? 
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        AddLines(Random.Range(0, 100) <= 50, xSize, ySize, map);
    }

    protected void AddLines(bool startAtTop, int xSize, int ySize, MapNode[,] map)
    {
        bool top = startAtTop;

        for (int x = 5; x < xSize; x += 5)
        {
            for (int y = 0; y < ySize - 4; y++)
            {
                int aY = top ? ySize - y - 1 : y;
                map[x, aY] = new MapNode(false, TileType.Valley);
            }
            top = !top;
        }
    }
}
