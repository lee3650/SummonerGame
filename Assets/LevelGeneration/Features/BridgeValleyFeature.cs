using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeValleyFeature : ValleyFeature
{
    protected override TileType GetValleyTile()
    {
        return TileType.Valley;
    }
    protected override bool IsValleyTileTraversable()
    {
        return false; 
    }
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        base.AddFeature(xSize, ySize, map);

        Vector2 bridgeWidth = Vector2.Perpendicular(BuildDirection());
        bridgeWidth.Normalize();

        MonoBehaviour.print("Bridge width: " + bridgeWidth);

        for (int x = 0; x < 3; x++)
        {
            Vector2 seed = ValleyCenters[Random.Range(0, ValleyCenters.Count)];

            if (Random.Range(0, 100) < 50f)
            {
                MakeThreeWideBridge(map, xSize, ySize, bridgeWidth, seed);
            } else
            {
                MakeOneWideBridge(map, seed);
            }
        }
    }

    void MakeThreeWideBridge(MapNode[,] map, int xSize, int ySize, Vector2 bridgeWidth, Vector2 seed)
    {
        Vector2 negDir = seed;
        Vector2 posDir = seed;

        for (int i = 0; i < 3; i++)
        {
            WriteDeltaPointToMap(negDir, xSize, ySize, bridgeWidth, map, GetBridgeType());
            WriteDeltaPointToMap(posDir, xSize, ySize, bridgeWidth, map, GetBridgeType());

            negDir -= BuildDirection();
            posDir += BuildDirection();
        }
    }

    void MakeOneWideBridge(MapNode[,] map, Vector2 seed)
    {
        Vector2 negDir = seed;
        Vector2 posDir = seed;

        for (int i = 0; i < 3; i++)
        {
            map[(int)negDir.x, (int)negDir.y] = GetBridgeType();
            map[(int)posDir.x, (int)posDir.y] = GetBridgeType();

            negDir -= BuildDirection();
            posDir += BuildDirection();
        }
    }

    MapNode GetBridgeType()
    {
        return new MapNode(true, TileType.Bridge);
    }
}
