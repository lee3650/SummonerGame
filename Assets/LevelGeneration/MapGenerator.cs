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

        AddDivider(xSize, ySize, newMap);

        AddWalls(newMap, xSize, ySize);

        //AddTotems(xSize, ySize, newMap);

        new ValleyExpander().AddFeature(xSize, ySize, newMap);
        
        return newMap;
    }

    void AddDivider(int xSize, int ySize, MapNode[,] newMap)
    {
        List<MapFeature> dividers = new List<MapFeature>()
        {
            new DividerFeature(),
            new LineDividerFeature(),
            new DoubleLineFeature(),
            new MazeFeature(),
            new MazeFeature(),
            new MazeFeature(),
            new MazeFeature(),
            new MazeFeature(),
            new MazeFeature(),
            new MazeFeature(),
        };

        MapFeature divider = dividers[Random.Range(0, dividers.Count)];

        divider.AddFeature(xSize, ySize, newMap);
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

    private void AddTotems(int xSize, int ySize, MapNode[,] newMap)
    {
        //might as well start at 1 because it's impossible to be 1 away from a wall in the bottom left corner. 

        List<TileType> typesToAvoid = new List<TileType>() {
            TileType.Wall,
            TileType.Valley,
            TileType.SummonTotem,
        };

        List<Vector2Int> prevTotems = new List<Vector2Int>();

        for (int y = 1; y < ySize - 1; y++)
        {
            for (int x = 1; x < xSize - 1; x++)
            {
                if (CanPlaceTotem(typesToAvoid, x, y, newMap, prevTotems))
                {
                    prevTotems.Add(new Vector2Int(x, y));
                    newMap[x, y] = new MapNode(true, TileType.SummonTotem);
                }
            }
        }
    }

    private bool CanPlaceTotem(List<TileType> typesToAvoid, int x, int y, MapNode[,] map, List<Vector2Int> prevTotems)
    {
        float minDist = 4.5f; 

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (typesToAvoid.Contains(map[x + dx, y + dy].TileType))
                {
                    return false; 
                }
            }
        }

        foreach (Vector2Int prevTotem in prevTotems)
        {
            if (Vector2Int.Distance(new Vector2Int(x, y), prevTotem) < minDist)
            {
                return false; 
            }
        }

        return true; 
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
        //this is actually pretty slow, right, because we do this for every tile and it probably has to write to memory to hold all these arguments

        float adjustedX = xSeed + ((float)x / xSize) * width;
        float adjustedY = ySeed + ((float)y / ySize) * width;

        float val = Mathf.PerlinNoise(adjustedX, adjustedY);

        //so, we're not going to return TileType.Marsh for now

        if (val < 0.70f)
        {
            return TileType.Land;
        }
        if (val < 0.85f)
        {
            if (!LetterManager.UseGameplayChange(GameplayChange.SandAndClearing)) //this is kinda sketchy but I guess it shows up in references, so it should be okay
            {
                return TileType.Land;
            }
            return TileType.Stone;
        }
        return TileType.Land;
    }
}
