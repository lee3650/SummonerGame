using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDelete : MonoBehaviour
{
    [SerializeField] TileType[] Tiles;
    [Tooltip("If the number of adjacent tiles in Tiles is >= DestroyThreshold, this object will be destroyed")]
    [SerializeField] int DestroyThreshold;

    public bool TryDestroy()
    {
        int count = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int cur = new Vector2Int(x, y) + new Vector2Int((int)transform.position.x, (int)transform.position.y);
                if (Contains(MapManager.GetTileType(cur)))
                {
                    count++;
                }
            }
        }
        if (count == DestroyThreshold)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            return true;  
        }
        return false; 
    }
    
    private bool Contains(TileType t)
    {
        foreach (TileType tile in Tiles)
        {
            if (t == tile)
            {
                return true;
            }
        }
        return false; 
    }
}
