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

        return newMap;
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
