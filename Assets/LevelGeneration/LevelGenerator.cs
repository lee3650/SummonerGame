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

    public const int MapStagesWidth = 3;
    public const int MapStagesHeight = 1;

    int maxLevel = 7; //starting at 1, let's say 7? Maybe 6? 

    //so, instead of directly setting the map in the map manager we want to copy it over. 
    public void GenerateNextLevel(int level, Vector2 pos, Vector2 delta)
    {
        levelNum = level;
        DestroyWalls(pos);

        List<MapFeature> features = GetMapFeatures(levelNum, delta);

        MapNode[,] newMap = MapGenerator.GenerateLevel(StageSize, StageSize, features);

        TransformOreBasedOnDistance(newMap, StageSize, StageSize, pos);

        CopyOverMap(newMap, pos);

        MapDrawer.InstantiatePartOfMap(MapManager.GetMap(), GetBottomLeftOfStage(pos), GetTopRightOfStage(pos));
    }

    public void RecalculateSpawnRegion(List<StageNode> endNodes)
    {
        WaveSpawner.ResetSpawnRegion();

        foreach (StageNode n in endNodes)
        {
            List<Vector2> spawnRegion = GenerateSpawnRegion(n.Position, n.Delta); //we'll come back to this. We're probably going to do it in the WaveSpawner. 
            RemoveInvalidTiles(spawnRegion, MapManager.GetMap());
            WaveSpawner.AddSpawnRegion(spawnRegion);
        }
    }

    List<Vector2> GenerateSpawnRegion(Vector2 sPos, Vector2 delta)
    {
        //so, the position doesn't actually matter, basically. 
        //only the delta, then we just have to adjust it based on the position. 
        //okay. 

        Vector2 bottomLeft;
        Vector2 topRight;

        int spawnRegWidth = 1;

        if (delta == new Vector2(1, 0))
        {
            bottomLeft = new Vector2(StageSize - spawnRegWidth, 0);
            topRight = new Vector2(StageSize, StageSize);
        }
        else if (delta == new Vector2(0, 1))
        {
            bottomLeft = new Vector2(0, StageSize - spawnRegWidth);
            topRight = new Vector2(StageSize, StageSize);
        }
        else if (delta == new Vector2(0, -1))
        {
            bottomLeft = new Vector2(0, 0);
            topRight = new Vector2(StageSize, spawnRegWidth);
        }
        else
        {
            throw new Exception("Did not expect delta " + delta);
        }

        topRight = GetTrueMapCoordinate((int)topRight.x, (int)topRight.y, sPos);
        bottomLeft = GetTrueMapCoordinate((int)bottomLeft.x, (int)bottomLeft.y, sPos);

        return GetPointsWithinBoundaries(bottomLeft, topRight);
    }

    void DestroyWalls(Vector2 position)
    {
        List<Vector2> wallTiles = GetPointsWithinBoundaries(GetBottomLeftOfStage(position), GetTopRightOfStage(position));
        //so, this is only the walls within the room. We also need to add the walls between rooms and write them as land... or I guess we write to every node in a map, and then we add extra walls 
        //around the entire thing. 

        print("walls to destroy: " + wallTiles.Count);

        MapDrawer.DestroyTiles(wallTiles);
    }

    void TransformOreBasedOnDistance(MapNode[,] newMap, int xSize, int ySize, Vector2 pos)
    {
        Vector2 centerTile = new Vector2(0f, (float)MapManager.ySize / 2);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (newMap[x, y].TileType == TileType.Ore)
                {
                    float distToSafety = Vector2.Distance(centerTile, GetTrueMapCoordinate(x, y, pos));

                    if (distToSafety < 15f)
                    {
                        newMap[x, y] = new MapNode(true, TileType.Copper);
                    } else if (distToSafety < 27f)
                    {
                        newMap[x, y] = new MapNode(true, TileType.Silver);
                    } else //so, we are going to have to change this when we add more ore types 
                    {
                        newMap[x, y] = new MapNode(true, TileType.Gold);
                    }
                }
            }
        }
    }

    public void SetTotalMapSizeAndInitMap()
    {
        MapManager.SetMapSize(new Vector2(MapStagesWidth * StageSize, MapStagesHeight * StageSize));
        MapManager.InitMap();
        MapDrawer.InitializeMap(); 
        MapDrawer.DrawEnclosingWalls(MapManager.xSize, MapManager.ySize);
    }

    void CopyOverMap(MapNode[,] map, Vector2 pos)
    {
        Vector2 bottomLeft = GetBottomLeftOfStage(pos);
        Vector2 topRight = GetTopRightOfStage(pos);

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

    Vector2 GetTrueMapCoordinate(int x, int y, Vector2 pos)
    {
        return new Vector2(x, y) + GetBottomLeftOfStage(pos);
    }

    Vector2 GetBottomLeftOfStage(Vector2 pos)
    {
        return pos * StageSize;
    }

    Vector2 GetTopRightOfStage(Vector2 pos)
    {
        return (pos + new Vector2(1, 1)) * StageSize;
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

    List<MapFeature> GetMapFeatures(int levelNum, Vector2 delta)
    {
        if (!ProgressionManager.UseGameplayChange(GameplayChange.SandAndClearing))
        {
            return new List<MapFeature>();
        }

        //so, these allow horizontal movement 
        List<MapFeature> HorizontalFeatures = new List<MapFeature>()
        {
            new ValleyFeature(),

        };

        //these allow vertical movement 
        List<MapFeature> VerticalFeatures = new List<MapFeature>()
        {
            new VerticalValleyFeature(),
        };

        //these allow any direction of movement 
        List<MapFeature> NeutralFeatures = new List<MapFeature>()
        {
            new LakeFeature(),
        };

        HorizontalFeatures.AddRange(NeutralFeatures);
        VerticalFeatures.AddRange(NeutralFeatures);

        int numOfFeatures = (int)UnityEngine.Random.Range(1, 4);

        //this is disgusting 
        List<MapFeature> featureList = null; 

        if (Mathf.Abs(delta.x) > 0)
        {
            featureList = HorizontalFeatures;
        }
        else if (Mathf.Abs(delta.y) > 0)
        {
            featureList = VerticalFeatures;
        }
        else
        {
            throw new Exception("Delta did not have x or y!");
            featureList = NeutralFeatures;
        }

        int highestFeature = UnityEngine.Random.Range(0, featureList.Count) + 1;

        List<MapFeature> result = new List<MapFeature>();

        for (int i = 0; i < numOfFeatures; i++)
        {
            MapFeature feature = featureList[UnityEngine.Random.Range(0, highestFeature)];

            if (feature is BridgeValleyFeature == false || FeatureListContainsBridge(result) == false)
            {
                result.Add(feature);
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
