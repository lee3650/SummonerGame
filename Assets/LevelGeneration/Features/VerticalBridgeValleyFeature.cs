using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBridgeValleyFeature : BridgeValleyFeature
{
    protected override Vector2 BuildDirection()
    {
        return new Vector2(0, 1);
    }
    
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        directions = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(1, -1),
        };
        base.AddFeature(xSize, ySize, map);
    }

    protected override Vector2 GetRandomStartPoint(int xSize, int ySize)
    {
        int startY = (int)Random.Range(ySize * .25f, ySize * 0.75f);
        return new Vector2(MapGenerator.WallWidth, startY);
    }
}
