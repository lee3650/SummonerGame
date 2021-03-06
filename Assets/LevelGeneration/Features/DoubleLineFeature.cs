using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLineFeature : LineDividerFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        AddLine(ySize / 3, xSize, map);
        AddLine(2 * ySize / 3, xSize, map);

        AddVerticalLine(xSize - lineXOffset - 1, ySize / 3, ySize, -1, map);
        AddVerticalLine(xSize - lineXOffset - 1, 2 * ySize / 3, ySize, 1, map);

    }
}
