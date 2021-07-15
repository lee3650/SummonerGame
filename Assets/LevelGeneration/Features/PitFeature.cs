using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFeature : LakeFeature
{
    protected override TileType GetLakeTileType()
    {
        return TileType.Valley;
    }
    protected override bool IsTraversable()
    {
        return false; 
    }
}
