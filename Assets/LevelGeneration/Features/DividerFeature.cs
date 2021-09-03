using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividerFeature : MapFeature
{
    //oh, so this takes x and y size... is it possible we planned for that to change? 
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        //so, the current plan is just a gap of two on either side? 
        //I'm basically planning to only do this... horizontally, for now. I kind of want the map to only go horizontally. Mm. We'll see, for sure. 
        /*
         * for (int x = 2; x < xSize - 2; x++)
        {
            map[x, ySize / 2] = new MapNode(false, TileType.Wall);
        }
         */

        bool top = false; 

        for (int x = 3; x < xSize; x += 4)
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
