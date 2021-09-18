using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public Vector2Int Point;
    public BlueprintType BlueprintType;
    public bool Satisfied;
    public float Rotation; 

    public Blueprint(Vector2Int point, BlueprintType blueprintType, float rotation)
    {
        Point = point;
        BlueprintType = blueprintType;
        Satisfied = false;
        Rotation = rotation;
    }
}
