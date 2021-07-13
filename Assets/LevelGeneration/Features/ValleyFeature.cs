using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValleyFeature : MapFeature
{
    static Vector2[] directions = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        };

    protected List<Vector2> ValleyCenters = new List<Vector2>();

    protected virtual TileType GetValleyTile()
    {
        return TileType.Water;
    }

    protected virtual bool IsValleyTileTraversable()
    {
        return true;
    }

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int startX = Random.Range(xSize/4, 3 * xSize/4);

        ValleyCenters = new List<Vector2>();

        Vector2 current = new Vector2(startX, 1);

        while (current.y < ySize - 1) //is it possible for this to cause issues because of float precision? Probably not. 
        {
            ValleyCenters.Add(current);
            if (IsPointOnMap(current, xSize, ySize))
            {
                map[(int)current.x, (int)current.y] = new MapNode(IsValleyTileTraversable(), GetValleyTile());
            }
            current += GetRandomUpwardDirection();
        }

        foreach (Vector2 vector2 in ValleyCenters)
        {
            if (IsPointOnMap(new Vector2(vector2.x + 1, vector2.y), xSize, ySize))
            {
                map[(int)vector2.x + 1, (int)vector2.y] = new MapNode(IsValleyTileTraversable(), GetValleyTile());
            }
            if (IsPointOnMap(new Vector2(vector2.x - 1, vector2.y), xSize, ySize))
            {
                map[(int)vector2.x - 1, (int)vector2.y] = new MapNode(IsValleyTileTraversable(), GetValleyTile());
            }
        }
    }

    static Vector2 GetRandomUpwardDirection()
    {
        return directions[Random.Range(0, directions.Length)];
    }
}