using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public Vector2Int Point;
    public BlueprintType BlueprintType;
    public bool Satisfied;
    public float Rotation;
    public float MaintenanceFee; 

    public Blueprint(Vector2Int point, BlueprintType blueprintType, float rotation, float maintenanceFee)
    {
        Point = point;
        BlueprintType = blueprintType;
        Satisfied = false;
        Rotation = rotation;
        MaintenanceFee = maintenanceFee;
    }
}
