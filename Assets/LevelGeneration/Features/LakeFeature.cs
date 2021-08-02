using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeFeature : MapFeature
{
    protected virtual bool IsTraversable()
    {
        return true; 
    }

    protected virtual float GetMinRadiusModifier()
    {
        return 0.1f;
    }

    protected virtual float GetMaxRadiusModifier()
    {
        return 0.25f; 
    }

    protected virtual TileType GetLakeTileType()
    {
        return TileType.Water;
    }

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        float radius = Random.Range(GetMinRadiusModifier() * xSize, xSize * GetMaxRadiusModifier());
        Vector2 center = new Vector2(Random.Range(0, xSize - MapGenerator.WallWidth), Random.Range(0, ySize - MapGenerator.WallWidth));

        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.CeilToInt(radius); x++)
        {
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.CeilToInt(radius); y++)
            {
                if (Vector2.Distance(center + new Vector2(x, y), center) <= radius)
                {
                    if (IsPointOnMap(center + new Vector2(x, y), xSize, ySize))
                    {
                        map[(int)center.x + x, (int)center.y + y] = new MapNode(IsTraversable(), GetLakeTileType());
                    }
                }
            }
        }
    }
}
