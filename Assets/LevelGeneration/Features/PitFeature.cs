using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFeature : LakeFeature
{
    protected override TileType GetLakeTileType()
    {
        return TileType.Valley;
    }

    protected override float GetMaxRadiusModifier()
    {
        return 0.1f;
    }

    protected override float GetMinRadiusModifier()
    {
        return 0.05f;
    }

    protected override bool IsTraversable()
    {
        return false; 
    }
}
