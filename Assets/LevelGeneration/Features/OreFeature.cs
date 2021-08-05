using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int seedNum = Random.Range(1, 4);

        List<Vector2> seeds = new List<Vector2>();

        for (int i = 0; i < seedNum; i++)
        {
            seeds.Add(new Vector2(Random.Range(0, xSize), Random.Range(0, ySize)));
        }

        Vector2[] growDirs = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
        };

        foreach (Vector2 seed in seeds)
        {
            Vector2 cur = seed; 

            map[(int)cur.x, (int)cur.y] = new MapNode(true, TileType.Ore);

            while(Random.Range(0, 100) < 30)
            {
                cur += growDirs[Random.Range(0, growDirs.Length)];
                if (IsPointOnMap(cur, xSize, ySize))
                {
                    map[(int)cur.x, (int)cur.y] = new MapNode(true, TileType.Ore);
                }
            }
        }
    }
}
