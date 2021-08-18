using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRestrictedSummon : SummonWeapon
{
    [SerializeField] List<TileType> RequiredTileTypes; 

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        Vector2 point = VectorRounder.RoundVector(mousePos);

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //so, just 'regular' adjacency - no diagonals and not the center tile. 
                if (y == 0 ^ x == 0) //^ = XOR
                {
                    if (MapManager.IsPointInBounds((int)point.x + x, (int)point.y + y))
                    {
                        if (RequiredTileTypes.Contains(MapManager.GetTileType(new Vector2(x + point.x, y + point.y))))
                        {
                            return true; 
                        }
                    }
                }
            }
        }

        return false; 
    }
}