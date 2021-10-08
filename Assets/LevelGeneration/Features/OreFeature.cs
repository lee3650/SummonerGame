using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int seedNum = Random.Range(3, 6);

        List<Vector2> seeds = new List<Vector2>();

        for (int i = 0; i < seedNum; i++)
        {
            seeds.Add(new Vector2(Random.Range(0, xSize), Random.Range(0, ySize)));
        }

        foreach (Vector2 seed in seeds)
        {
            Vector2 cur = seed; 

            map[(int)cur.x, (int)cur.y] = new MapNode(true, TileType.Ore);
        }
    }
}
