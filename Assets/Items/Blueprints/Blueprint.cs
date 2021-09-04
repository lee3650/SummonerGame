using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public Vector2 Point;
    public BlueprintType BlueprintType;
    public bool Satisfied; 

    public Blueprint(Vector2 point, BlueprintType blueprintType)
    {
        Point = point;
        BlueprintType = blueprintType;
        Satisfied = false; 
    }
}
