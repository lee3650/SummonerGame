using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure8 : DonutFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int h1 = Mathf.RoundToInt(Random.Range(2 * xSize / 3f, 3 * xSize / 4f));
        int h2 = Mathf.RoundToInt((int)Random.Range(xSize / 4f, xSize / 3f));

        int k = ySize / 2;

        int a1 = xSize - h1;
        int b1 = Random.Range((k - 2), k + 1);

        int a2 = (h1 - a1) - h2; //so, the radius of the left donut is 
                                //the difference between the leftmost point of the right donut and the center of the left donut 
        int b2 = Random.Range((k - 2), k + 1);

        DrawEllipse(a1, b1, k, h1, map, TileType.Land, true);
        DrawEllipse(a2, b2, k, h2, map, TileType.Land, true);

        //draw the hole 
        DrawEllipse(a1 - 2, b1 - 2, k, h1, map, TileType.DoNotDraw, false);
        DrawEllipse(a2 - 2, b2 - 2, k, h2, map, TileType.DoNotDraw, false);
    }
}
