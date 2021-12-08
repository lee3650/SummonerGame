using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuityManager : MonoBehaviour, IResettable
{
    private static bool[,] Continuous;

    private static Vector2Int[] adjs = new Vector2Int[] //how many times will I make this array lol 
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
    };

    public static void CalculateContinuity(MapNode[,] map)
    {
        Continuous = new bool[map.GetLength(0), map.GetLength(1)];

        List<Vector2> spawns = LevelGenerator.GetValidSpawns(map);
        Vector2Int start = VectorRounder.RoundVectorToInt(spawns[Random.Range(0, spawns.Count)]);
        DFS(start, map);
    }

    private static void DFS(Vector2Int pos, MapNode[,] map)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2Int cur = pos + adjs[i];
            if (!IsPointContinuous(cur) && MapFeature.InBounds(map, cur) && map[cur.x, cur.y].IsTraversable(true))
            {
                Continuous[cur.x, cur.y] = true;
                DFS(cur, map);
            }
        }
    }

    public static bool IsPointContinuous(Vector2Int point)
    {
        if (point.x >= 0 && point.x < Continuous.GetLength(0) && point.y >= 0 && point.y < Continuous.GetLength(1))
        {
            return Continuous[point.x, point.y];
        }
        return false;
    }

    public void ResetState()
    {

    }
}
