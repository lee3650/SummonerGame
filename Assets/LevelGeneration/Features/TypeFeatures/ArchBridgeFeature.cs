using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchBridgeFeature : ArchipelagoFeature
{
    protected override void AddBridge(Vector2Int cur, Vector2Int w1, IslandNode par, IslandNode child, MapNode[,] map)
    {
        BridgeAdder.AddBridge(cur, w1.x != 0); //so, w1 is perpendicular to the direction of the bridge, that's why it's != 0
    }

    protected override int GetDist(Vector2Int delta, int size, int childSize)
    {
        int result = base.GetDist(delta, size, childSize);
        if (delta.x != 0)
        {
            return result + 3;
        }
        return result; 
    }
}
