using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int seedNum = 4;

        List<Vector2Int> seeds = new List<Vector2Int>();

        for (int i = 0; i < seedNum; i++)
        {
            bool add = false;

            Vector2Int pos = new Vector2Int(Random.Range(0, xSize), Random.Range(0, ySize));
            if (map[pos.x, pos.y].TileType != TileType.DoNotDraw)
            {
                add = true;
            } else
            {
                int undrawnTiles = 0;

                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -2; y <= 2; y++)
                    {
                        Vector2Int c = pos + new Vector2Int(x, y);
                        if (InBounds(map, c))
                        {
                            if (map[c.x, c.y].TileType == TileType.DoNotDraw)
                            {
                                undrawnTiles++;
                            }
                        }
                    }
                }

                if (undrawnTiles == 25)
                {
                    add = true;


                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            Vector2Int c = pos + new Vector2Int(x, y);
                            map[c.x, c.y] = new MapNode(true, TileType.Land);
                        }
                    }
                }
            }

            if (add)
            {
                seeds.Add(pos);
            }
        }

        //okay. So, 4 randomly placed, then let's do 3 purposefully or guaranteed placed. 

        for (int i = 0; i < 3; i++)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, xSize), Random.Range(0, ySize));
            while (map[pos.x, pos.y].TileType == TileType.DoNotDraw || seeds.Contains(pos))
            {
                pos = new Vector2Int(Random.Range(0, xSize), Random.Range(0, ySize));
            }

            seeds.Add(pos);
        }

        foreach (Vector2Int seed in seeds)
        {
            Vector2Int cur = seed; 

            map[cur.x, cur.y] = new MapNode(true, TileType.Ore);
        }
    }
}
