using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapFeature
{
    public abstract MapNode[,] GetFeature(Vector2 size);
}