using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointCommand : PlayerCommand
{
    public HoldPointCommand(Vector2 point)
    {
        PointToHold = point; 
    }

    public Vector2 PointToHold
    {
        get;
        set;
    }
}
