using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRestrictedSummon : SummonWeapon
{
    [SerializeField] List<TileType> RequiredTileTypes;
    [Tooltip("Tiles that this tile cannot be spawned next to")]
    [SerializeField] List<TileType> BlacklistedTileTypes;
    [SerializeField] Summoner Summoner; //no clue if that's the right class

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        Vector2 point = VectorRounder.RoundVector(mousePos);

        BlueprintBarracks bb;
        if (Summon.TryGetComponent<BlueprintBarracks>(out bb))
        {
            Vector2 spawnPoint = point + bb.GetSpawnOffset();

            return (CanAcceptPoint(point) || CanAcceptPoint(spawnPoint)) && ((CanOverridePoint(point)) && CanOverridePoint(spawnPoint));
        
        } else
        {
            return CanAcceptPoint(point) && CanOverridePoint(point);
        }
    }

    private bool CanAcceptPoint(Vector2 point)
    {
        if (!CanOverridePoint(point))
        {
            return false;
        }

        bool acceptedTiles = GetAcceptedTiles(point);

        if (AreTilesBlacklisted(point))
        {
            return false;
        }

        return acceptedTiles;
    }

    private bool CanOverridePoint(Vector2 point)
    {
        if (IsPointOre(point))
        {
            return false;
        }

        if (IsPointBuilding(point))
        {
            return false; 
        }

        if (BlueprintBarracks.IsPointSpawnPoint(VectorRounder.RoundVectorToInt(point)))
        {
            return false;
        }

        if (!MapManager.IsPointTraversable(point, false))
        {
            return false; 
        }

        return true; 
    }

    private bool IsPointBuilding(Vector2 point)
    {
        TileType t = MapManager.GetTileType(point);
        return MapNode.IsTileTypeBuilding(t);

    }

    private bool IsPointOre(Vector2 point)
    {
        TileType under = MapManager.GetTileType(point);
        if (MapNode.IsTileOre(under))
        {
            return true;
        }
        return false; 
    }

    private bool GetAcceptedTiles(Vector2 point)
    {
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
                            return true; 
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool AreTilesBlacklisted(Vector2 point)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //so, just 'regular' adjacency - no diagonals and not the center tile. 
                if (y == 0 ^ x == 0) //^ = XOR - wow, I can't believe I used that. 
                {
                    if (MapManager.IsPointInBounds((int)point.x + x, (int)point.y + y))
                    {
                        if (IsAdjacentTileBlacklisted(point + new Vector2(x, y)))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false; 
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