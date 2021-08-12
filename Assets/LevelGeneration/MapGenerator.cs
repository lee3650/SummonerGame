using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public const int WallWidth = 0; 

    //so, this should take instead a list of 'features' 
    public MapNode[,] GenerateLevel(int xSize, int ySize, List<MapFeature> features)
    {
        MapNode[,] newMap = new MapNode[xSize, ySize];

        InitializeMap(newMap, xSize, ySize);

        foreach (MapFeature feature in features)
        {
            feature.AddFeature(xSize, ySize, newMap);
        }

        new OreFeature().AddFeature(xSize, ySize, newMap);

        AddWalls(newMap, xSize, ySize);

        return newMap;
    }

    void AddWalls(MapNode[,] newMap, int xSize, int ySize)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int i = 0; i < WallWidth; i++)
            {
                newMap[x, i] = new MapNode(false, TileType.Wall);
                newMap[x, ySize - i - 1] = new MapNode(false, TileType.Wall);
            }
        }

        for (int y = 0; y < ySize; y++)
        {
            for (int i = 0; i < WallWidth; i++)
            {
                newMap[i, y] = new MapNode(false, TileType.Wall);
                newMap[xSize - 1 - i, y] = new MapNode(false, TileType.Wall);
            }
        }
    }

    //okay. So, this is the only place we really have to do this. 
    //Instead of doing it as it is, we're going to randomly choose between
    //regular land, marsh, stones, and hills. 
    //that's kind of a lot, but whatever.
    //So, let's use perlin noise for this, eh? 
    void InitializeMap(MapNode[,] map, int xSize, int ySize)
    {
        float xSeed = Random.Range(0, 100f);
        float ySeed = Random.Range(0, 100f);

        float width = Random.Range(5, 10f);

        //so, we want the entire thing to be roughly 5-10 in total, right
        //so we should adjust our coordinate a little... 

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                map[x, y] = new MapNode(true, GetRandomTileType(x, y, xSize, ySize, xSeed, ySeed, width));
            }
        }
    }

    TileType GetRandomTileType(int x, int y, int xSize, int ySize, float xSeed, float ySeed, float width)
    {
        //wow that's a lot of arguments 
        float adjustedX = xSeed + ((float)x / xSize) * width;
        float adjustedY = ySeed + ((float)y / ySize) * width;

        float val = Mathf.PerlinNoise(adjustedX, adjustedY);

        if (val <= 0.25f)
        {
            return TileType.Marsh;
        }
        if (val < 0.75f)
        {
            return TileType.Land;
        }
        if (val < 0.88f)
        {
            return TileType.Stone;
        }
        return TileType.Hills;
    }
}
