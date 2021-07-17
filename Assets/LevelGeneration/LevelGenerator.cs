using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] MapGenerator MapGenerator;
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] MapDrawer MapDrawer;
    private int levelNum = 0;

    int maxLevel = 7; //starting at 1, let's say 7? Maybe 6? 

    public void GenerateNextLevel(int level)
    {
        levelNum = level; 

        MapDrawer.DestroyOldMap();

        Vector2 mapSize = GetMapSize(levelNum);

        MapManager.SetMapSize(mapSize);

        List<MapFeature> features = GetMapFeatures(levelNum);

        //we need to generate a list of features based on the level number 
        MapNode[,] newMap = MapGenerator.GenerateLevel((int)mapSize.x, (int)mapSize.y, features);

        List<Vector2> spawnRegion = GenerateSpawnRegion(mapSize, levelNum); //this will have to depend on the terrain - we don't want to spawn entities on an invalid tile. 

        RemoveInvalidTiles(spawnRegion, newMap); 

        WaveSpawner.SetSpawnRegion(spawnRegion);

        MapManager.SetMap(newMap);

        MapDrawer.InstantiateMap(newMap);

        //levelNum++; 
    }

    void RemoveInvalidTiles(List<Vector2> spawnRegion, MapNode[,] map)
    {
        for (int i = spawnRegion.Count - 1; i >= 0; i--)
        {
            if (map[(int)spawnRegion[i].x, (int)spawnRegion[i].y].Traversable == false)
            {
                spawnRegion.RemoveAt(i);
            } 
        }
    }

    //so, this isn't deterministic, which I guess is good. 
    Vector2 GetMapSize(int levelNum)
    {
        int startHeight = Random.Range(10, 25);

        float widthMultiplier = Mathf.Lerp(Random.Range(1.5f, 2.5f), Random.Range(0.9f, 1.2f), LevelPercentage(levelNum));

        return new Vector2(startHeight * widthMultiplier, startHeight); 
    }

    List<Vector2> GenerateSpawnRegion(Vector2 mapSize, int levelNum)
    {
        //okay let's work up to 3. But only at the very end, I guess. 
        int maxSpawnZones = 3;

        //probably just do this procedurally. That's the easiest way. 

        int spawns = Mathf.RoundToInt(Mathf.Lerp(1, maxSpawnZones, Mathf.Pow(LevelPercentage(levelNum), 4)));

        List<Vector2> result = GetRightSpawn(mapSize);
        if (spawns > 1)
        {
            result.AddRange(GetUpperSpawn(mapSize));
        }
        if (spawns > 2)
        {
            result.AddRange(GetLeftSpawn(mapSize));
        }

        return result; 
    }

    List<MapFeature> GetMapFeatures(int levelNum)
    {
        //so, we want to order these from making it easier to making it harder or neutral. 
        List<MapFeature> Features = new List<MapFeature>()
        {
            new LakeFeature(),
            new PitFeature(),
            new ValleyFeature(),
        };

        int numOfFeatures = (int)Mathf.Lerp(Random.Range(1, 4), 1, LevelPercentage(levelNum));
        int highestFeature = Mathf.RoundToInt(Mathf.Lerp(Features.Count, 0, Mathf.Pow(LevelPercentage(levelNum), 3)));

        List<MapFeature> result = new List<MapFeature>();

        for (int i = 0; i < numOfFeatures; i++)
        {
            result.Add(Features[Random.Range(0, highestFeature)]); //this is exclusive
        }

        foreach (MapFeature feature in result)
        {
            print("feature type: " + feature.GetType());
        }

        return result; 
    }
    
    List<Vector2> GetRightSpawn(Vector2 mapSize)
    {
        Vector2 bottomLeft = new Vector2(mapSize.x - 3, 1);
        Vector2 topRight = new Vector2(mapSize.x - 1, mapSize.y - 1);
        return GetPointsWithinBoundaries(bottomLeft, topRight);
    }

    List<Vector2> GetLeftSpawn(Vector2 mapSize)
    {
        Vector2 bottomLeft = new Vector2(1, 1);
        Vector2 topRight = new Vector2(4, mapSize.y - 1);
        return GetPointsWithinBoundaries(bottomLeft, topRight);
    }

    List<Vector2> GetUpperSpawn(Vector2 mapSize)
    {
        Vector2 bottomLeft = new Vector2(1, mapSize.y - 3);
        Vector2 topRight = new Vector2(mapSize.x - 1, mapSize.y - 1);
        return GetPointsWithinBoundaries(bottomLeft, topRight);
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
