using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandAndClearingFeature : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        float res = Random.Range(1f, 2f);

        Vector2 offset = new Vector2(Random.Range(0f, 10000f), Random.Range(0f, 10000f));

        int clearings = Random.Range(5, 8);

        for (int i = 0; i < clearings; i++)
        {
            Vector2Int rand = new Vector2Int(Random.Range(0, xSize), Random.Range(0, ySize));
            while (map[rand.x, rand.y].TileType == TileType.DoNotDraw)
            {
                rand = new Vector2Int(Random.Range(0, xSize), Random.Range(0, ySize));
            }
            map[rand.x, rand.y] = new MapNode(true, TileType.Stone);
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (map[x, y].TileType != TileType.DoNotDraw && map[x, y].TileType != TileType.Ore && map[x, y].TileType != TileType.Stone)
                {
                    float val = Mathf.PerlinNoise(x * res + offset.x, y * res + offset.y);

                    MonoBehaviour.print("perlin val " + val + ", at " + new Vector2(x * res + offset.x, y * res + offset.y));

                    if (val > 0.42f)
                    {
                        map[x, y] = new MapNode(true, TileType.Water);
                    }
                }
            }
        }
    }
}
