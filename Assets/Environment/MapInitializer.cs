using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitializer : MonoBehaviour
{
    private void Awake()
    {
        for (int x = 0; x < MapManager.xSize; x++)
        {
            for (int y = 0; y < MapManager.ySize; y++)
            {
                MapManager.WritePoint(x, y, new MapNode(true));

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero); 
                if (hit)
                {
                    if (hit.transform.CompareTag("Untraversable"))
                    {
                        MapManager.WritePoint(x, y, new MapNode(false));
                    } 
                }
            }
        }

        MapManager.PrintMap();
    }
}
