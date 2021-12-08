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
    
    public const int MapWidth = 45;
    public const int MapHeight = 15;

    public const int maxLevel = 7;

    public void GenerateNextLevel()
    {
        MapNode[,] newMap = MapGenerator.GenerateLevel(MapWidth, MapHeight);

        TransformOreBasedOnDistance(newMap); //we can do this somewhere else if we need to. 

        CopyOverMap(newMap);

        MapDrawer.InstantiatePartOfMap(MapManager.GetMap(), new Vector2(0, 0), new Vector2(MapWidth, MapHeight));
    }

    public void RecalculateSpawnRegion() //potentially, we could spawn in any direction now. 
    {
        WaveSpawner.ResetSpawnRegion();

        List<Vector2> spawnRegion = GetValidSpawns(MapManager.GetMap());

        WaveSpawner.AddSpawnRegion(spawnRegion);
    }
    
    public static List<Vector2> GetValidSpawns(MapNode[,] map)
    {
        List<Vector2> spawnRegion = GenerateSpawnRegion(); //we'll come back to this. We're probably going to do it in the WaveSpawner. 
        RemoveInvalidTiles(spawnRegion, map);
        return spawnRegion;
    }

    /*
    void DestroyWalls(Vector2 position)
    {
        List<Vector2> wallTiles = GetPointsWithinBoundaries(GetBottomLeftOfStage(position), GetTopRightOfStage(position));
        //so, this is only the walls within the room. We also need to add the walls between rooms and write them as land... or I guess we write to every node in a map, and then we add extra walls 
        //around the entire thing. 

        print("walls to destroy: " + wallTiles.Count);

        MapDrawer.DestroyTiles(wallTiles);
    }
     */ 

    void TransformOreBasedOnDistance(MapNode[,] newMap)
    {
        Vector2 centerTile = new Vector2(0f, (float)MapHeight / 2);

        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                if (newMap[x, y].TileType == TileType.Ore)
                {
                    float distToSafety = Vector2.Distance(centerTile, new Vector2(x, y));
                    /*
                     *   if (distToSafety < 15f)
                    {
                        newMap[x, y] = new MapNode(true, TileType.Copper);
                    } else
                     * 
                     */
                    if (distToSafety < 27f)
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
        MapManager.SetMapSize(new Vector2(MapWidth, MapHeight));
        MapManager.InitMap();
        MapDrawer.InitializeMap(); 
        //MapDrawer.DrawEnclosingWalls(MapManager.xSize, MapManager.ySize);
    }

    void CopyOverMap(MapNode[,] map)
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                MapManager.WritePoint(x, y, map[x, y]);
            }
        }
    }
    
    private static void RemoveInvalidTiles(List<Vector2> spawnRegion, MapNode[,] map)
    {
        for (int i = spawnRegion.Count - 1; i >= 0; i--)
        {
            if (map[(int)spawnRegion[i].x, (int)spawnRegion[i].y].IsTraversable(true) == false)
            {
                spawnRegion.RemoveAt(i);
            } 
        }
    }

    private static List<Vector2> GenerateSpawnRegion()
    {
        return GetPointsWithinBoundaries(new Vector2(MapWidth - 1, 0), new Vector2(MapWidth, MapHeight));
    }

    List<MapFeature> GetMapFeatures(int levelNum, Vector2 delta)
    {
        if (!LetterManager.UseGameplayChange(GameplayChange.SandAndClearing))
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
