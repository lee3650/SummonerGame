using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        //okay so the equation is 
        //((x - h)^2 / a^2) + ((y - k)^2 / b^2) = 1
        //a = x radius
        //b = y radius
        //h, k = center. 

        int h = Random.Range(xSize / 2, 2 * xSize / 3);
        int k = ySize / 2; //this is kinda sus, but whatever 

        int a = xSize - h;
        int b = Random.Range((ySize - 3) / 2, (ySize / 2));

        DrawEllipse(a, b, k, h, map, TileType.Land, true);

        a -= 3;
        b -= 3;

        DrawEllipse(a, b, k, h, map, TileType.DoNotDraw, false);
    }

    protected void DrawEllipse(int a, int b, int k, int h, MapNode[,] map, TileType tile, bool traversable)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if ((Mathf.Pow((x - h), 2) / (a * a)) + (Mathf.Pow((y - k), 2) / (b * b)) <= 1)
                {
                    map[x, y] = new MapNode(traversable, tile);
                }
            }
        }
    }
}
