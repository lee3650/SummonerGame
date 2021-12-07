using System.Collections;
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

        new OreFeature().AddFeature(xSize, ySize, newMap);

        MapFeature divider = GetDividerForType(type);

        AddDivider(newMap, divider);

        if (isDesert)
        {
            new DesertFeature().AddFeature(xSize, ySize, newMap); 
        }

        new ValleyExpander().AddFeature(xSize, ySize, newMap); //oh that's cool. 

        //okay so we'll also have to make a continuity manager no matter what, so keep that in mind. 

        ContinuityManager.CalculateContinuity();

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
            return MapType.Rectangle;
        }

        return MapType.Archipelago;

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
            case MapType.Rectangle:
                return new RectFeature();
                /*
            case MapType.Donut:
                return new DonutFeature();
            case MapType.Ellipse:
                return new EllipseFeature();
            case MapType.Tree:
                return new TreeFeature();
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
            case MapType.Rectangle:
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
                map[x, y] = new MapNode(false, TileType.DoNotDraw);//GetRandomTileType(x, y, xSize, ySize, xSeed, ySeed, width));
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
