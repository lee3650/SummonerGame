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
            Vector2 negDir = seed;
            Vector2 posDir = seed;
         
            for (int i = 0; i < 3; i++)
            {

                //technically, we don't have to do it this way because the valley is always 3 wide, but better not to assume that. 
                
                WriteDeltaPointToMap(negDir, xSize, ySize, bridgeWidth, map, new MapNode(true, TileType.Bridge));
                WriteDeltaPointToMap(posDir, xSize, ySize, bridgeWidth, map, new MapNode(true, TileType.Bridge));

                negDir -= BuildDirection();
                posDir += BuildDirection();

            }
        }
    }
}
