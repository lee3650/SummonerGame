using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //so, this should take instead a list of 'features' 
    public MapNode[,] GenerateLevel(int xSize, int ySize, List<MapFeature> features)
    {
        MapNode[,] newMap = new MapNode[xSize, ySize];

        InitializeMap(newMap, xSize, ySize);

        foreach (MapFeature feature in features)
        {
            feature.AddFeature(xSize, ySize, newMap);
        }

        AddWalls(newMap, xSize, ySize);

        return newMap;
    }

    void AddWalls(MapNode[,] newMap, int xSize, int ySize)
    {
        for (int x = 0; x < xSize; x++)
        {
            newMap[x, 0] = new MapNode(false, TileType.Wall);
            newMap[x, ySize - 1] = new MapNode(false, TileType.Wall);
        }

        for (int y = 0; y < ySize; y++)
        {
            newMap[0, y] = new MapNode(false, TileType.Wall);
            newMap[xSize - 1, y] = new MapNode(false, TileType.Wall);
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
