using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] MapGenerator MapGenerator;
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] MapDrawer MapDrawer;
    private int levelNum = 0;

    public const int StageSize = 15; 

    int maxLevel = 7; //starting at 1, let's say 7? Maybe 6? 

    //so, instead of directly setting the map in the map manager we want to copy it over. 
    public void GenerateNextLevel(int level, LevelDirections direction)
    {
        levelNum = level;
        DestroyWalls(direction);

        List<MapFeature> features = GetMapFeatures(levelNum, direction);

        MapNode[,] newMap = MapGenerator.GenerateLevel(StageSize, StageSize, features);

        TransformOreBasedOnDistance(newMap, StageSize, StageSize, direction);

        List<Vector2> spawnRegion = GenerateSpawnRegion(direction);

        CopyOverMap(newMap, direction);

        RemoveInvalidTiles(spawnRegion, MapManager.GetMap());
        WaveSpawner.AddSpawnRegion(spawnRegion);

        MapDrawer.InstantiatePartOfMap(MapManager.GetMap(), GetBottomLeftOfStage(direction), GetTopRightOfStage(direction));
    }

    void DestroyWalls(LevelDirections dir)
    {
        List<Vector2> wallTiles = GetPointsWithinBoundaries(GetBottomLeftOfStage(dir), GetTopRightOfStage(dir));
        //so, this is only the walls within the room. We also need to add the walls between rooms and write them as land... or I guess we write to every node in a map, and then we add extra walls 
        //around the entire thing. 

        print("walls to destroy: " + wallTiles.Count);

        MapDrawer.DestroyTiles(wallTiles);
    }

    void TransformOreBasedOnDistance(MapNode[,] newMap, int xSize, int ySize, LevelDirections dir)
    {
        Vector2 centerTile = new Vector2((float)MapManager.xSize / 2, (float)MapManager.ySize / 2);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (newMap[x, y].TileType == TileType.Ore)
                {
                    float distToSafety = 0;
                    if (dir == LevelDirections.Center)
                    {
                        distToSafety = Vector2.Distance(centerTile, GetTrueMapCoordinate(x, y, dir));
                    } else if (dir == LevelDirections.East || dir == LevelDirections.West)
                    {
                        distToSafety = Mathf.Abs(centerTile.x - GetTrueMapCoordinate(x, y, dir).x);
                    } else if (dir == LevelDirections.North || dir == LevelDirections.South)
                    {
                        distToSafety = Mathf.Abs(centerTile.y - GetTrueMapCoordinate(x, y, dir).y);
                    }

                    if (distToSafety < 7f)
                    {
                        newMap[x, y] = new MapNode(true, TileType.Copper);
                    } else if (distToSafety < 12f)
                    {
                        newMap[x, y] = new MapNode(true, TileType.Silver);
                    } else
                    {
                        newMap[x, y] = new MapNode(true, TileType.Gold);
                    }
                }
            }
        }
    }

    public void SetTotalMapSizeAndInitMap()
    {
        MapManager.SetMapSize(new Vector2(3 * StageSize, 3 * StageSize));
        MapManager.InitMap();
        MapDrawer.InstantiateMap(MapManager.GetMap());
        MapDrawer.DrawEnclosingWalls(MapManager.xSize, MapManager.ySize);
    }

    void CopyOverMap(MapNode[,] map, LevelDirections dir)
    {
        Vector2 bottomLeft = GetBottomLeftOfStage(dir);
        Vector2 topRight = GetTopRightOfStage(dir);

        int minX = (int)bottomLeft.x;
        int minY = (int)bottomLeft.y;

        for (int x = minX; x < topRight.x; x++)
        {
            for (int y = minY; y < topRight.y; y++)
            {
                MapManager.WritePoint(x, y, map[x - minX, y - minY]);
            }
        }
    }

    Vector2 GetTrueMapCoordinate(int x, int y, LevelDirections mapDir)
    {
        return new Vector2(x, y) + GetBottomLeftOfStage(mapDir);
    }

    Vector2 GetBottomLeftOfStage(LevelDirections dir)
    {
        switch (dir)
        {
            case LevelDirections.Center:
                return new Vector2(1, 1) * StageSize;
            case LevelDirections.East:
                return new Vector2(2, 1) * StageSize;
            case LevelDirections.North:
                return new Vector2(1, 2) * StageSize;
            case LevelDirections.South:
                return new Vector2(1, 0) * StageSize;
            case LevelDirections.West:
                return new Vector2(0, 1) * StageSize;
        }
        throw new System.Exception("Invalid direction " + dir);
    }

    Vector2 GetTopRightOfStage(LevelDirections dir)
    {
        switch (dir)
        {
            case LevelDirections.Center:
                return new Vector2(2, 2) * StageSize;
            case LevelDirections.East:
                return new Vector2(3, 2) * StageSize;
            case LevelDirections.North:
                return new Vector2(2, 3) * StageSize;
            case LevelDirections.South:
                return new Vector2(2, 1) * StageSize;
            case LevelDirections.West:
                return new Vector2(1, 2) * StageSize;
        }
        throw new System.Exception("Invalid direction " + dir);
    }

    void RemoveInvalidTiles(List<Vector2> spawnRegion, MapNode[,] map)
    {
        for (int i = spawnRegion.Count - 1; i >= 0; i--)
        {
            if (map[(int)spawnRegion[i].x, (int)spawnRegion[i].y].IsTraversable(true) == false)
            {
                spawnRegion.RemoveAt(i);
            } 
        }
    }

    List<Vector2> GenerateSpawnRegion(LevelDirections roomDirection)
    {
        List<Vector2> result = new List<Vector2>();
        
        switch (roomDirection)
        {
            case LevelDirections.Center:
                break;    
            case LevelDirections.West:
                result = GetPointsWithinBoundaries(new Vector2(0, StageSize), new Vector2(2, StageSize * 2));
                break;
            case LevelDirections.East:
                result = GetPointsWithinBoundaries(new Vector2(StageSize * 3 - 2, StageSize), new Vector2(3 * StageSize, StageSize * 2));
                break;
            case LevelDirections.North:
                result = GetPointsWithinBoundaries(new Vector2(StageSize, 3 * StageSize - 2), new Vector2(2 * StageSize, StageSize * 3));
                break;
            case LevelDirections.South:
                result = GetPointsWithinBoundaries(new Vector2(StageSize, 0), new Vector2(2 * StageSize, 2));
                break; 
        }
        return result; 
    }

    List<MapFeature> GetMapFeatures(int levelNum, LevelDirections dir)
    {
        //so, these allow horizontal movement 
        List<MapFeature> HorizontalFeatures = new List<MapFeature>()
        {
            new ValleyFeature(),
            new BridgeValleyFeature(),

        };

        //these allow vertical movement 
        List<MapFeature> VerticalFeatures = new List<MapFeature>()
        {
            new VerticalValleyFeature(),
            new VerticalBridgeValleyFeature(),
        };

        //these allow any direction of movement 
        List<MapFeature> NeutralFeatures = new List<MapFeature>()
        {
            new LakeFeature(),
            new PitFeature(),
        };

        HorizontalFeatures.AddRange(NeutralFeatures);
        VerticalFeatures.AddRange(NeutralFeatures);

        int numOfFeatures = (int)UnityEngine.Random.Range(1, 4);

        //this is disgusting 
        List<MapFeature> featureList = null; 
        switch (dir)
        {
            case LevelDirections.Center:
                featureList = NeutralFeatures;
                break;
            case LevelDirections.East:
            case LevelDirections.West:
                featureList = HorizontalFeatures;
                break; 
            case LevelDirections.North:
            case LevelDirections.South:
                featureList = VerticalFeatures;
                break; 
        }

        int highestFeature = UnityEngine.Random.Range(0, featureList.Count) + 1;
        
        List<MapFeature> result = new List<MapFeature>();

        for (int i = 0; i < numOfFeatures; i++)
        {
            MapFeature feature = featureList[UnityEngine.Random.Range(0, highestFeature)];

            if (feature is BridgeValleyFeature == false || FeatureListContainsBridge(result) == false)
            {
                result.Add(feature); //this is exclusive
            }
        }

        foreach (MapFeature feature in result)
        {
            print("feature type: " + feature.GetType());
        }

        return result; 
    }

    bool FeatureListContainsBridge(List<MapFeature> features)
    {
        foreach (MapFeature f in features)
        {
            if (f is BridgeValleyFeature)
            {
                return true; 
            }
        }
        return false; 
    }

    public static List<Vector2> GetPointsWithinBoundaries(Vector2 bottomLeft, Vector2 topRight)
    {
        List<Vector2> result = new List<Vector2>();

        for (int x = (int)bottomLeft.x; x < topRight.x; x++)
        {
            for (int y = (int)bottomLeft.y; y < topRight.y; y++)
            {
                result.Add(new Vector2(x, y));
            }
        }

        return result;
    }

    public float LevelPercentage(int levelNum)
    {
        return (float)((float)levelNum / (float)maxLevel);
    }
}
