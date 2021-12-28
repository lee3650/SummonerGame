using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToHoldManager : MonoBehaviour
{
    private Vector2 point;
    private bool initialized = false;

    public Vector2 PointToHold
    {
        get
        {
            return point;
        }
        set
        {
            initialized = true; 
            point = value;
        }
    }

    public bool Initialized
    {
        get
        {
            return initialized;
        }
    }
}
