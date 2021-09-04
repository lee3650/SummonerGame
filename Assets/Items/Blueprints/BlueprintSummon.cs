using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintSummon
{
    public BlueprintType BlueprintType;
    public Vector2 Point;
    public HealthManager HealthManager;

    public bool IsAlive()
    {
        return HealthManager.IsAlive();
    }

    public BlueprintSummon(HealthManager hm, Blueprint bp)
    {
        Point = bp.Point; 
        BlueprintType = bp.BlueprintType;
        HealthManager = hm; 
    }
}
