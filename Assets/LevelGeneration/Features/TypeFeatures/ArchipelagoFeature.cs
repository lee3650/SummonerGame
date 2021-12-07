using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchipelagoFeature : MapFeature
{
    const int maxYDelta = 9;
    const int maxXDelta = 15;

    private Vector2Int[] Dirs = new Vector2Int[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };

    private Vector2Int[] Corners = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
    };

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        int spawnIslands = Random.Range(1, 3);

        List<IslandNode> seeds = new List<IslandNode>();

        for (int i = 0; i < spawnIslands; i++)
        {
            int delta = ChooseSize();
            int firstY = Random.Range(delta, ySize - delta);

            seeds.Add(new IslandNode(new Vector2Int(xSize - delta, firstY), delta, null));
            FillOutIslandNode(seeds[i], map);
        }

        for (int i = 0; i < seeds.Count; i++)
        {
            BuildChain(map, seeds[i]);
        }
    }

    private void FillOutIslandNode(IslandNode node, MapNode[,] map)
    {
        for (int x = -node.Size; x <= node.Size; x++)
        {
            for (int y = -node.Size; y <= node.Size; y++)
            {
                Vector2Int mapPos = new Vector2Int(x, y) + node.Position;
                if (InBounds(map, mapPos))
                {
                    map[mapPos.x, mapPos.y] = new MapNode(true, TileType.Land);
                }
            }
        }
    }

    private void BuildChain(MapNode[,] map, IslandNode parent)
    {
        MonoBehaviour.print("Building chain! Position: " + parent.Position);

        int n = Random.Range(1, 4);

        Vector2Int[] dirs = new Vector2Int[n];

        Dirs = (Vector2Int[])ListRandomSort<Vector2Int>.SortListRandomly(Dirs);

        for (int i = 0; i < n; i++)
        {
            dirs[i] = Dirs[i];
        }

        List<IslandNode> children = new List<IslandNode>();
        parent.Next = children;

        for (int i = 0; i < n; i++)
        {
            Vector2Int delta = Dirs[Random.Range(0, Dirs.Length)];
            int size = ChooseSize();
            Vector2Int pos = (delta * GetDist(delta, parent.Size, size)) + parent.Position; 

            if (PositionAvailable(map, pos, size))
            {
                IslandNode child = new IslandNode(pos, size, parent);
                children.Add(child);
                FillOutIslandNode(child, map);
                DrawBridgeBetweenIslands(map, parent, child);
                BuildChain(map, child); 
            }
        }
    }

    private void DrawBridgeBetweenIslands(MapNode[,] map, IslandNode parent, IslandNode child)
    {
        //so, basically we draw a 3x3 based on the difference in the positions. It should be, once normalized 
        //well, it's one of three directions. 

        Vector2 delta = ((Vector2)(child.Position - parent.Position)).normalized;

        Vector2Int dint = VectorRounder.RoundVectorToInt(delta);

        if (delta.magnitude < 1)
        {
            throw new System.Exception("Delta magnitude was less than 1!");
        }

        Vector2Int w1 = new Vector2Int(dint.y, dint.x);
        Vector2Int w2 = -w1;

        Vector2Int cur = parent.Position;

        while (cur != child.Position)
        {
            MonoBehaviour.print("Drawing bridge! Position: " + parent.Position);

            map[cur.x, cur.y] = new MapNode(true, TileType.Land); //land for now, bridge later, I guess
            map[cur.x + w1.x, cur.y + w1.y] = new MapNode(true, TileType.Land); //land for now, bridge later, I guess
            map[cur.x + w2.x, cur.y + w2.y] = new MapNode(true, TileType.Land); //land for now, bridge later, I guess

            cur += dint;
        }
    }

    private bool PositionAvailable(MapNode[,] map, Vector2Int pos, int size)
    {
        for (int i = 0; i < 4; i++)
        {
            if (!InBounds(map, pos + size * Corners[i]))
            {
                return false;
            } 
        }

        return true; 
    }

    private int ChooseSize()
    {
        return Random.Range(1, 3);
    }

    private int GetDist(Vector2Int delta, int size, int childSize)
    {
        if (delta.x != 0)
        {
            return Random.Range(3 + size + childSize, maxXDelta);
        } else
        {
            return Random.Range(3 + size + childSize, maxYDelta);
        }
    }
}