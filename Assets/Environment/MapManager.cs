using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
    public static int xSize = 36;
    public static int ySize = 17;

    static MapNode[,] Map = new MapNode[xSize, ySize]; //I believe all these entries are automatically null. 

    public static void InitMap()
    {
        Map = new MapNode[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Map[x, y] = new MapNode(false, TileType.Wall);
            }
        }
    }

    public static void SetMapSize(Vector2 mapSize)
    {
        xSize = (int)mapSize.x;
        ySize = (int)mapSize.y;
    }

    public static bool IsPointTraversable(Vector2 point, bool CanGoThroughWalls)
    {
        return IsPointTraversable(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), CanGoThroughWalls);
    }

    public static MapNode ReadPoint(int x, int y)
    {
        return Map[x, y];
    }

    public static Vector2 GetClosestValidTile(Vector2 start)
    {
        start = VectorRounder.RoundVector(start);

        float minDistance = Mathf.Infinity;
        Vector2 result = start; 

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (IsPointTraversable(x, y, false))
                {
                    if (Vector2.Distance(new Vector2(x, y), start) < minDistance)
                    {
                        minDistance = Vector2.Distance(new Vector2(x, y), start);
                        result = new Vector2(x, y);
                    }
                }
            }
        }

        return result;
    }

    public static void SetConnectedToOcean(Vector2Int tile)
    {
        if (IsPointInBounds(tile.x, tile.y))
        {
            Map[tile.x, tile.y].ConnectedToOcean = true;
        }
    }

    public static bool IsTileConnectedToOcean(Vector2Int tile)
    {
        if (IsPointInBounds(tile.x, tile.y))
        {
            return Map[tile.x, tile.y].ConnectedToOcean || Map[tile.x, tile.y].TileType == TileType.Wall;
        }
        return true; 
    }

    public static int GetTraversalCost(int x, int y)
    {
        return Map[x, y].TraversalCost;
    }

    public static void PrintMap()
    {
        string result = "";

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                if (IsPointTraversable(x, y, false))
                {
                    result += "0";
                } else
                {
                    result += "1";
                }
            }
            result += "\n";
        }

        MonoBehaviour.print(result);
    }

    public static bool IsMapInitialized()
    {
        return Map != null && Map[0, 0] != null;
    }

    public static TileType GetTileType(Vector2 point)
    {
        if (IsPointInBounds((int)point.x, (int)point.y))
        {
            return Map[(int)point.x, (int)point.y].TileType;
        }
        if (point.x < 0)
        {
            return TileType.LeftOfMap;
        }
        if (point.x >= xSize)
        {
            return TileType.RightOfMap;
        }
        if (point.y > ySize)
        {
            return TileType.TopOfMap;
        }
        return TileType.BottomOfMap;
    }

    public static bool IsPointTraversable(int x, int y, bool CanGoThroughWalls)
    {
        if (x >= xSize || y >= ySize || x < 0 || y < 0)
        {
            return false;
        }

        return Map[x, y].IsTraversable(CanGoThroughWalls);
    }

    public static bool IsTileType(int x, int y, TileType tileType)
    {
        if (x >= xSize || y >= ySize || x < 0 || y < 0)
        {
            return false;
        }

        return Map[x, y].TileType == tileType;
    }

    public static List<TileType> GetAdjacentTiles(Vector2Int tile)
    {
        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
        };

        List<TileType> result = new List<TileType>();

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int cur = tile + dirs[i];
            if (IsPointInBounds(cur.x, cur.y))
            {
                result.Add(Map[cur.x, cur.y].TileType);
            }
        }

        return result; 
    }

    public static bool IsPointInBounds(int x, int y)
    {
        if (x >= xSize || y >= ySize || x < 0 || y < 0 || Map[0,0] == null)
        {
            return false;
        }
        return true; 
    }

    public static int GetNumOfAdjacentImpassableTiles(int sx, int sy)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 ^ y != 0)
                {
                    if (IsPointInBounds(sx + x, sy + y))
                    {
                        if (!Map[sx + x, sy + y].IsTraversable(false))
                        {
                            count++; //holy nesting 
                        }
                    }
                }
            }
        }

        return count; 
    }

    public static void WritePoint(int x, int y, MapNode newNode)
    {
        if (x >= xSize || y >= ySize || x < 0 || y < 0)
        {
            return;
        }

        Map[x, y] = newNode;
    }

    public static MapNode[,] GetMap()
    {
        return Map;
    }
}
