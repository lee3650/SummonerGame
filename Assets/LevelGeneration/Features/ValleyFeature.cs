using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using UnityEngine;

public class ValleyFeature : MapFeature
{
    protected Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        };

    protected virtual Vector2 GetRandomStartPoint(int xSize, int ySize)
    {
        int startX = Random.Range(xSize / 4, 3 * xSize / 4);
        return new Vector2(startX, MapGenerator.WallWidth);
    }

    protected List<Vector2> ValleyCenters = new List<Vector2>();

    protected virtual TileType GetValleyTile()
    {
        return TileType.Water;
    }

    protected virtual Vector2 BuildDirection()
    {
        return new Vector2(1, 0);
    }

    protected virtual bool IsValleyTileTraversable()
    {
        return true;
    }

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        Vector2 start = GetRandomStartPoint(xSize, ySize);

        ValleyCenters = GetValleyCenters(xSize, ySize, start, map);

        WriteSurroundingPointsToMap(xSize, ySize, map);
    }

    void WriteSurroundingPointsToMap(int xSize, int ySize, MapNode[,] map)
    {
        foreach (Vector2 vector2 in ValleyCenters)
        {
            WriteDeltaPointToMap(vector2, xSize, ySize, BuildDirection(), map, new MapNode(IsValleyTileTraversable(), GetValleyTile()));
        }
    }

    protected void WriteDeltaPointToMap(Vector2 point, int xSize, int ySize, Vector2 BuildDir, MapNode[,] map, MapNode nodeToWrite)
    {
        if (IsPointOnMap(point, xSize, ySize))
        {
            map[(int)point.x, (int)point.y] = nodeToWrite;
        }

        if (IsPointOnMap(point + BuildDir, xSize, ySize))
        {
            map[(int)point.x + (int)BuildDir.x, (int)point.y + (int)BuildDir.y] = nodeToWrite;
        }

        if (IsPointOnMap(point - BuildDir, xSize, ySize))
        {
            map[(int)point.x - (int)BuildDir.x, (int)point.y - (int)BuildDir.y] = nodeToWrite;
        }
    }

    protected List<Vector2> GetValleyCenters(int xSize, int ySize, Vector2 start, MapNode[,] map)
    {
        List<Vector2> valleyCenters = new List<Vector2>();

        Vector2 current = start;

        while (current.y < ySize - MapGenerator.WallWidth && current.x < xSize - MapGenerator.WallWidth) //is it possible for this to cause issues because of float precision? Probably not. 
        {
            valleyCenters.Add(current);
            current += GetRandomValleyDirection();
        }

        return valleyCenters;
    }

    Vector2 GetRandomValleyDirection()
    {
        return directions[Random.Range(0, directions.Length)];
    }
}