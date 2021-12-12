using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEllipseFeature : MapFeature
{
    private Vector2Int[] Dirs = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
    };

    const int NodeSize = 5;

    Vector2Int[,] MazeMap;

    float h, a, k, b;

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        //so, the equation: ((x - h)^2 / a^2) + ((y - k)^2 / b^2) = 1
        //where h is the center.x
        //k is the center.y
        //a is the x rad, and b is the y rad

        //okay so basically
        //to represent the map, we can just divide ySize / 5 and xSize / 5
        //and round up, right... I think. 
        //and then, um, go from there. 
        //okay fine let's not round up

        MazeMap = new Vector2Int[xSize / NodeSize, ySize / NodeSize];

        Vector2Int mazeSize = new Vector2Int(MazeMap.GetLength(0), MazeMap.GetLength(1));

        h = mazeSize.x / 2;
        a = mazeSize.x - h;

        k = mazeSize.y / 2;
        b = mazeSize.y / 2;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                map[x, y] = new MapNode(false, TileType.DoNotDraw);
            }
        }

        BuildMaze(new Vector2Int(mazeSize.x - 1, mazeSize.y / 2), map);

        /*
        for (int y = 0; y < MazeMap.GetLength(1); y++)
        {
            for (int x = 0; x < MazeMap.GetLength(0); x++)
            {
                MonoBehaviour.print(string.Format("Maze node at ({0}, {1}) is {2}", x, y, MazeMap[x, y]));
                if (MazeMap[x, y] != new Vector2Int())
                {
                    DrawMazeNode(new Vector2Int(x, y), MazeMap[x, y], map);
                }
            }
        }
         */
    }

    private void DrawMazeNode(Vector2Int pos, Vector2Int dir, MapNode[,] map)
    {
        Vector2Int mapPos = pos * NodeSize + new Vector2Int(4, 2); 

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                //Note: add constant 1. This shifts everything over right by 1, because everything is adjusted left 1 because it expects everything to be surrounded 
                //by donotdraw. 
                if (InBounds(map, new Vector2Int(mapPos.x + x, mapPos.y + y))) 
                { 
                    map[mapPos.x + x, mapPos.y + y] = new MapNode(true, TileType.Land);
                }
                if (InBounds(map, new Vector2Int(mapPos.x + x + dir.x, mapPos.y + y + dir.y)))
                {
                    map[mapPos.x + x + dir.x, mapPos.y + y + dir.y] = new MapNode(true, TileType.Land);
                }
                if (InBounds(map, new Vector2Int(mapPos.x + x + 2 * dir.x, mapPos.y + y + 2 * dir.y)))
                {
                    map[mapPos.x + x + 2 * dir.x, mapPos.y + y + 2 * dir.y] = new MapNode(true, TileType.Land);
                }
            }
        }
    }

    private void BuildMaze(Vector2Int pos, MapNode[,] map)
    {
        Dirs = (Vector2Int[])ListRandomSort<Vector2Int>.SortListRandomly(Dirs);
        for (int i = 0; i < Dirs.Length; i++)
        {
            Vector2Int cand = Dirs[i] + pos;
            if (CanBuildPos(cand)) 
            {
                MazeMap[pos.x, pos.y] = Dirs[i];
                DrawMazeNode(pos, Dirs[i], map);
                MazeMap[cand.x, cand.y] = -Dirs[i];
                DrawMazeNode(cand, -Dirs[i], map);
                BuildMaze(cand, map);
            }
        }
    }

    private bool CanBuildPos(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < MazeMap.GetLength(0) && pos.y >= 0 && pos.y < MazeMap.GetLength(1))
        {
            return MazeMap[pos.x, pos.y] == new Vector2Int() && InEllipseBounds(pos);
        }
        return false; 
    }

    private bool InEllipseBounds(Vector2Int pos)
    {
        float score = (Mathf.Pow(pos.x - h, 2) / Mathf.Pow(a, 2)) + (Mathf.Pow(pos.y - k, 2) / Mathf.Pow(b, 2));
        MonoBehaviour.print("pos: " + pos + ", val: " + score);
        return (Mathf.Pow(pos.x - h, 2) / Mathf.Pow(a, 2)) + (Mathf.Pow(pos.y - k, 2) / Mathf.Pow(b, 2)) <= 1.3f;
    }
}

class MazeNode
{
    public int Center; 

    public MazeNode()
    {

    }
}