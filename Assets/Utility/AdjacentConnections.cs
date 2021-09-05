using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public class AdjacentConnections : MonoBehaviour
{
    static TileType[] ConnectedTiles = new TileType[] { 
        TileType.Barracks,
        TileType.ArcherBarracks,
        TileType.Miner,
        TileType.WallGenerator, //okay so now I have to change this in two places. Mm. 
    };


    static Vector2Int[] dirs = new Vector2Int[]
    {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
    };
    
    static TileType goal = TileType.Miner;

    public static bool DoAdjacentTilesConnectToMiner(Vector2Int start)
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            //so, if this tile is traversable, and it's not a miner, then we'll test it, blacklisting ourselves. 
            Vector2Int cur = start + dirs[i];

            if (IsNodeTileAcceptable(cur))
            {
                if (!MapManager.IsTileType(cur.x, cur.y, goal))
                {
                    bool r = ExistsPathToGoal(cur, start);
                    if (!r)
                    {
                        return false; 
                    }
                }
            }
        }

        return true; 
    }

    static bool ExistsPathToGoal(Vector2Int start, Vector2Int blacklist) 
    {
        //wait, this doesn't make sense. 
        //we have to do 4 start points. 

        List<Vector2Int> closedList = new List<Vector2Int>();
        closedList.Add(blacklist);

        List<Vector2Int> openList = new List<Vector2Int>();

        openList.Add(start);

        while (openList.Count != 0)
        {
            Vector2Int next = openList[openList.Count - 1];
            openList.RemoveAt(openList.Count - 1);

            bool r = ExpandAdjacents(next, openList, closedList);

            if (r)
            {
                return true; 
            }
        }

        return false; 
    }

    public static bool ExpandAdjacents(Vector2Int pointToExpand, List<Vector2Int> openList, List<Vector2Int> closedList)
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int next = pointToExpand + dirs[i];
            if (CanTraverseNode(next, openList, closedList))
            {
                if (MapManager.IsTileType(next.x, next.y, goal))
                {
                    return true;
                } else
                {
                    openList.Add(next);
                }
            }
        }

        closedList.Add(pointToExpand);

        return false; 
    }

    private static bool CanTraverseNode(Vector2Int point, List<Vector2Int> openList, List<Vector2Int> closedList)
    {
        if (!IsNodeTileAcceptable(point))
        {
            return false; 
        }
        
        if (closedList.Contains(point)) //I'm not sure how .equals works for vector2int. It's probably fine. 
        {
            return false; 
        }

        if (openList.Contains(point))
        {
            return false; //we don't need to go to it again if we already have it. 
        }

        return true; 
    }

    private static bool IsNodeTileAcceptable(Vector2Int point)
    {
        if (!MapManager.IsPointInBounds(point.x, point.y))
        {
            return false;
        }

        TileType t = MapManager.GetTileType(point);

        if (!ConnectedTiles.Contains(t))
        {
            return false;
        }
        return true; 
    }
}
