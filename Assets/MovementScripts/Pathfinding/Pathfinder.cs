using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    const int TileSize = 1;

    public static SearchNode GetPathFromPointToPoint(Vector2 start, Vector2 end)
    {
        start = new Vector2(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y));
        end = new Vector2(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));

        if (MapManager.IsPointTraversable(start) == false || MapManager.IsPointTraversable(end) == false)
        {
            print("The start or end was not valid!");
            return null; 
        }



        SearchNode openHead;

        openHead = new SearchNode((int)start.x, (int)start.y);
        openHead.f = 0f; 

        while (openHead != null)
        {
            SearchNode q = openHead;

            openHead = openHead.NextNode;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {

                }
            }
        }
    }
}
