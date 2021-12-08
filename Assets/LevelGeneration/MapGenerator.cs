﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public const int WallWidth = 0; 

    public MapNode[,] GenerateLevel(int xSize, int ySize)
    {
        MapNode[,] newMap = new MapNode[xSize, ySize];

        InitializeMap(newMap, xSize, ySize);

        MapType type = ChooseMapType();
        
        InitializeMapType(type, newMap);

        bool isDesert = ChooseIsDesert(); 

        List<MapFeature> features = GetFeaturesForType(type);

        foreach (MapFeature feature in features)
        {
            feature.AddFeature(xSize, ySize, newMap);
        }

        new SandAndClearingFeature().AddFeature(xSize, ySize, newMap);

        new OreFeature().AddFeature(xSize, ySize, newMap);

        MapFeature divider = GetDividerForType(type);

        AddDivider(newMap, divider);

        if (isDesert)
        {
            new DesertFeature().AddFeature(xSize, ySize, newMap); 
        }

        //new ValleyExpander().AddFeature(xSize, ySize, newMap);  
        return newMap;
    }

    private bool ChooseIsDesert()
    {
        //check gameplay changes! 

        if (MainMenuScript.TutorialMode)
        {
            return false;
        }

        return Random.Range(0, 100) < 25;
    }

    private MapType ChooseMapType()
    {
        if (MainMenuScript.TutorialMode)
        {
            return MapType.Archipelago;
        }

        return MapType.Bridged;

        MapType[] types = (MapType[])System.Enum.GetValues(typeof(MapType));
        MapType type = types[Random.Range(0, types.Length)];

        return type; 
    }

    private void InitializeMapType(MapType type, MapNode[,] map)
    {
        MapFeature typeFeature = GetTypeFeature(type);
        typeFeature.AddFeature(LevelGenerator.MapWidth, LevelGenerator.MapHeight, map);
    }

    private MapFeature GetTypeFeature(MapType type)
    {
        //check gameplay changes! 

        switch (type)
        {
            case MapType.Archipelago:
                return new ArchipelagoFeature();
            case MapType.Bridged:
                return new ArchBridgeFeature();
                /*
            case MapType.Donut:
                return new DonutFeature();
            case MapType.Bridged:
                return new BridgeFeature();
                 */
        }

        throw new System.Exception("Could not find type feature for " + type);
    }

    private List<MapFeature> GetFeaturesForType(MapType type)
    {
        if (MainMenuScript.TutorialMode)
        {
            return new List<MapFeature>();
        }

        switch (type)
        {
            case MapType.Archipelago:
                return new List<MapFeature>();
            case MapType.Bridged:
                return new List<MapFeature>();
        }

        throw new System.Exception("Could not find regular features for " + type);
    }

    private MapFeature GetDividerForType(MapType type)
    {
        //throw new System.NotImplementedException();
        return new EmptyDivider();
    }

    void AddDivider(MapNode[,] newMap, MapFeature divider)
    {
        divider.AddFeature(newMap.GetLength(0), newMap.GetLength(1), newMap);
    }

    void InitializeMap(MapNode[,] map, int xSize, int ySize)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                map[x, y] = new MapNode(false, TileType.DoNotDraw); 
            }
        }
    }
}
