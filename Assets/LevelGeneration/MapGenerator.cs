using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void InitializeMap(MapNode[,] map, int xSize, int ySize)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                map[x, y] = new MapNode(true, TileType.Land);
            }
        }
    }
}
