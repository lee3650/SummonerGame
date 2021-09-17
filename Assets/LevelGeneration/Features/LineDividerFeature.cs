using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDividerFeature : MapFeature
{
    protected int lineXOffset = 2; 

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        AddLine(ySize / 2, xSize, map);
        int buildDir = Random.Range(0, 100) > 50 ? -1 : 1; 
        AddVerticalLine(xSize - lineXOffset - 1, ySize/2, ySize, buildDir, map);
        int secondYStart = -buildDir * 3 + (ySize / 2); 
        AddVerticalLine(lineXOffset, secondYStart, ySize, -buildDir, map);
    }

    protected void AddLine(int y, int xSize, MapNode[,] map)
    {
        for (int x = lineXOffset; x < xSize - lineXOffset; x++)
        {
            map[x, y] = new MapNode(false, TileType.Valley);
        }
    }

    protected void AddVerticalLine(int x, int startY, int ySize, int buildDir, MapNode[,] map)
    {
        for (int y = startY; HorizontalLineConditional(y, ySize); y += buildDir)
        {
            map[x, y] = new MapNode(false, TileType.Valley);
        }
    }

    private bool HorizontalLineConditional(int y, int ySize)
    {
        return y < ySize && y >= 0;
    }
}
