using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedMazeFeature : DividerFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        AddLines(true, xSize, ySize, map);
    }
}
