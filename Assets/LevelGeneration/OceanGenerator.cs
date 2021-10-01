using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    [SerializeField] MapDrawer MapDrawer;
    [SerializeField] int buffer = 4; //buffer is per side. 
    [SerializeField] TileType[] Blacklist;

    private Vector2Int[] Adjacents = new Vector2Int[9]
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(1, -1),
        new Vector2Int(1, 1),
    };

    public void DrawOcean()
    {
        MapNode[,] oceanMap = new MapNode[MapManager.xSize + 2 * buffer, MapManager.ySize + 2 * buffer];
        int xSize = MapManager.xSize + 2 * buffer;
        int ySize = MapManager.ySize + 2 * buffer;

        //step 1: fill with ocean tiles
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                oceanMap[x, y] = new MapNode(false, TileType.OceanTile);
            }
        }

        //step 2: fill every tile with transition tiles + 1 tile buffer if it's.. basically not a wall. 
        //I guess we can destroy valleys too then. 

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int i = 0; i < Adjacents.Length; i++)
                {
                    if (!BlacklistContains(MapManager.GetTileType(AdjustMapPosition(new Vector2Int(x, y) + Adjacents[i])))) 
                    {
                        //if there is a single, non-blacklisted tile in there
                        //set it to an ocean transition, not an ocean tile

                        oceanMap[x, y] = new MapNode(false, TileType.OceanTransition);

                    }
                }
            }
        }

        MapDrawer.InstantiateNonCenteredMap(oceanMap, new Vector2Int(-buffer, -buffer)); 
        //the nice thing about this is we can use it to draw islands too, since this can draw any tile
    }

    private bool BlacklistContains(TileType t)
    {
        foreach (TileType tile in Blacklist)
        {
            if (t == tile)
            {
                return true;
            }
        }
        return false; 
    }

    private Vector2Int AdjustMapPosition(Vector2Int cur) 
    {
        return new Vector2Int(-buffer, -buffer) + cur; 
    }
}
