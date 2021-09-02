using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRestrictedSummon : SummonWeapon
{
    [SerializeField] List<TileType> RequiredTileTypes;
    [Tooltip("Tiles that this tile cannot be spawned next to")]
    [SerializeField] List<TileType> BlacklistedTileTypes; 

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        Vector2 point = VectorRounder.RoundVector(mousePos);

        bool acceptedTiles = false;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //so, just 'regular' adjacency - no diagonals and not the center tile. 
                if (y == 0 ^ x == 0) //^ = XOR - wow, I can't believe I used that. 
                {
                    if (MapManager.IsPointInBounds((int)point.x + x, (int)point.y + y))
                    {
                        if (IsAdjacentTileAccepted(point + new Vector2(x, y)))
                        {
                            acceptedTiles = true;
                        }
                        if (IsAdjacentTileBlacklisted(point + new Vector2(x, y)))
                        {
                            return false; 
                        }
                    }
                }
            }
        }

        return acceptedTiles;
    }

    bool IsAdjacentTileAccepted(Vector2 point)
    {
        return RequiredTileTypes.Contains(MapManager.GetTileType(point)); 
    }

    bool IsAdjacentTileBlacklisted(Vector2 point)
    {
        return BlacklistedTileTypes.Contains(MapManager.GetTileType(point));
    }
}