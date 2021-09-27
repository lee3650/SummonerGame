using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyExpander : MapFeature
{
    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (map[x, y].TileType == TileType.Valley)
                {
                    MakeValley2x2(xSize, ySize, map, x, y);
                }
            }
        }
    }

    private void MakeValley2x2(int xSize, int ySize, MapNode[,] map, int x, int y)
    {
        Vector2Int[] corners = new Vector2Int[4]
        {
            new Vector2Int(0, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0,-1),
        };

        Vector2Int[] pointsPerCorner = new Vector2Int[4]
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
        };

        int[] scores = new int[4];

        for (int i = 0; i < corners.Length; i++)
        {
            Vector2Int cur = corners[i] + new Vector2Int(x, y);
            bool outOfBounds = false; 
            if (WithinBounds(cur.x, cur.y, xSize, ySize))
            {
                for (int j = 0; j < pointsPerCorner.Length; j++)
                {
                    if (!outOfBounds)
                    {
                        Vector2Int pos = pointsPerCorner[j] + cur;
                        if (WithinBounds(pos.x, pos.y, xSize, ySize))
                        {
                            if (map[pos.x, pos.y].TileType == TileType.Valley)
                            {
                                scores[i] += 1;
                                if (scores[i] == 4)
                                {
                                    return; 
                                }
                            }
                        } else
                        {
                            outOfBounds = true;
                            scores[i] = -1; //so, score at i, not j
                        }
                    }
                }
            }
        }

        //okay so we've constructed the scores array. 
        //we know that none of them are 4, as well. 
        int maxIndex = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > scores[maxIndex])
            {
                maxIndex = i;
            } 
        }

        for (int i = 0; i < pointsPerCorner.Length; i++)
        {
            Vector2Int cur = pointsPerCorner[i] + corners[maxIndex] + new Vector2Int(x, y);
            map[cur.x, cur.y] = new MapNode(false, TileType.Valley);
        }
    }

    private bool WithinBounds(int x, int y, int xSize, int ySize)
    {
        return x >= 0 && x < xSize && y >= 0 && y < ySize; 
    }
}
