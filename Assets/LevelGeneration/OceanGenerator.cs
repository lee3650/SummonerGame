using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OceanGenerator : MonoBehaviour
{
    [SerializeField] MapDrawer MapDrawer;
    [SerializeField] int buffer = 12; //buffer is per side. 
    [SerializeField] TileType[] Blacklist;
    [SerializeField] TileType[] Whitelist;

    private Vector2Int[] Adjacents = new Vector2Int[14]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 1),
        new Vector2Int(2, 0),
        new Vector2Int(-2, 0),
        new Vector2Int(0, 2),
        new Vector2Int(-2, 2),
        new Vector2Int(2, 2),
    };

    private Vector2Int[] DirAdj = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 1),
    };

    public void DrawOcean()
    {
        MapNode[,] oceanMap = new MapNode[MapManager.xSize + 2 * buffer, MapManager.ySize + 2 * buffer];
        int xSize = MapManager.xSize + 2 * buffer;
        int ySize = MapManager.ySize + 2 * buffer;

        print("x size: " + xSize + "; y size: " + ySize);

        //step 1: fill with ocean tiles
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Vector2Int adjust = AdjustMapPosition(new Vector2Int(x, y));
                bool use = MapManager.NotPartOfMap(adjust);

                if (adjust.y >= 15)
                {
                    print("y is " + y + ", use: " + use);
                } 

                if (use) 
                {
                    oceanMap[x, y] = new MapNode(false, TileType.OceanTile);
                } else
                {
                    oceanMap[x, y] = new MapNode(false, TileType.DoNotDraw);
                }
            }
        }

        //step 1.5: put land edge tiles in to transition from the land to the ocean
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int i = 0; i < DirAdj.Length; i++)
                {
                    if (AddWallTile(AdjustMapPosition(new Vector2Int(x, y) + Adjacents[i]), AdjustMapPosition(new Vector2Int(x, y))))
                    {
                        oceanMap[x, y] = new MapNode(false, TileType.LandEdge);
                        break;
                    }
                }
            }
        }

        //step 2: fill every tile with transition tiles + 1 tile buffer if it's.. basically not a wall. 
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (AddTransitionTile(new Vector2Int(x, y), oceanMap))
                {
                    oceanMap[x, y] = new MapNode(false, TileType.OceanTransition);
                }
            }
        }

        //step 3: put in some little islands - this definitely could cause issues. 
        //okay we need a random tile that isn't near the 'real' map... 
        int islandCount = Random.Range(3, 6); 

        for (int i = 0; i < islandCount; i++)
        {
            Vector2Int sizes = new Vector2Int(xSize, ySize);
            int islandSize = Random.Range(1, 3); //basically 1 or 2.
            Vector2Int seed = GetSeed(sizes, (islandSize * islandSize) + 2);
            FillOutIslandSeed(oceanMap, seed, sizes, islandSize);
        }

        //writeMap(oceanMap, "finalmap");

        MapDrawer.InstantiateNonCenteredMap(oceanMap, new Vector2Int(-buffer, -buffer)); 
    }

    public static void writeMap(MapNode[,] map, string message)
    {
        string result = message + "\n\n\n";

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                result += map[x, y].TileType.ToString().Substring(0, 1) + " ";
            }
            result += "\n";
        }

        File.WriteAllText("C:/Users/ignor/Documents/ArchDebug/" + message + ".txt", result);
    }

    private void FillOutIslandSeed(MapNode[,] oceanMap, Vector2Int seed, Vector2Int sizes, int islandSize) 
    {
        //okay.
        //now this can assume those conditions hold. Hopefully it looks good lol. Way to put the code over the client. 

        print("seed: " + seed + ", island size: " + islandSize);

        for (int x = -1; x <= 1 + islandSize; x++)
        {
            for (int y = -1 - islandSize; y <= 1; y++)
            {
                if (seed.x + x < 0 || seed.x + x >= sizes.x || seed.y + y < 0 || seed.y + y >= sizes.y)
                {
                    throw new System.Exception("Point " + new Vector2Int(seed.x + x, seed.y + y) + " was out of range!");
                }
                if (oceanMap[seed.x + x, seed.y + y].TileType != TileType.IslandTile) 
                {
                    oceanMap[seed.x + x, seed.y + y] = new MapNode(false, TileType.OceanTransition);
                }
            }
        }

        for (int x = 0; x <= islandSize; x++)
        {
            for (int y = 0; y >= -islandSize; y--)
            {
                oceanMap[seed.x + x, seed.y + y] = new MapNode(false, TileType.IslandTile);
            }
        }
    }

    public Vector2Int GetSeed(Vector2Int sizes, int rightAndBelowSpace)
    {
        //4 options - basically 4 'zones' for this. 
        //x = 0 to buffer/2, y = 0 to mapsize. 
        //x = xSize - buffer/2 to xSize, y = 0 to mapsize
        //y = 0 to buffer/2, x = 0 to mapsize
        //y = ySize - buffer/2 to mapsize, x = 0 to mapsize. 

        //okay. 
        int leftAndAboveSpace = 3;
        int ran = Random.Range(0, 3);

        //case 2:
        //  return new Vector2Int(Random.Range(sizes.x - buffer + leftAndAboveSpace, sizes.x - rightAndBelowSpace), Random.Range(rightAndBelowSpace, sizes.y - leftAndAboveSpace));

        switch (ran)
        {
            case 0:
                return new Vector2Int(Random.Range(leftAndAboveSpace, (buffer - rightAndBelowSpace)), Random.Range(rightAndBelowSpace, sizes.y - leftAndAboveSpace));
            case 1:
                return new Vector2Int(Random.Range(leftAndAboveSpace, sizes.x - rightAndBelowSpace), Random.Range(rightAndBelowSpace, buffer - leftAndAboveSpace));
            default:
                return new Vector2Int(Random.Range(leftAndAboveSpace, sizes.x - rightAndBelowSpace), Random.Range(sizes.y - buffer + rightAndBelowSpace, sizes.y - leftAndAboveSpace));
        }
    }

    private bool AddWallTile(Vector2Int adjustedPosition, Vector2Int originalPosition)
    {
        if (MapManager.IsPointTraversable(adjustedPosition, true) && MapManager.NotPartOfMap(originalPosition))
        {
            return true;
        }
        return false; 
    }

    private bool AddTransitionTile(Vector2Int originalPosition, MapNode[,] oceanMap)
    {
        Vector2Int adj = AdjustMapPosition(originalPosition);

        if (!MapManager.NotPartOfMap(adj))
        {
            return false;
        }

        if (oceanMap[originalPosition.x, originalPosition.y].TileType == TileType.LandEdge)
        {
            return false;
        }

        for (int i = 0; i < Adjacents.Length; i++)
        {
            Vector2Int cur = originalPosition + Adjacents[i];
            if (MapFeature.InBounds(oceanMap, cur) && oceanMap[cur.x, cur.y].TileType == TileType.LandEdge)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2Int AdjustMapPosition(Vector2Int cur) 
    {
        return new Vector2Int(-buffer, -buffer) + cur; 
    }
}
