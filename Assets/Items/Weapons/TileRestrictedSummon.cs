using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRestrictedSummon : SummonWeapon
{
    [SerializeField] List<TileType> RequiredTileTypes;
    [Tooltip("Tiles that this tile cannot be spawned next to")]
    [SerializeField] List<TileType> BlacklistedTileTypes;
    [SerializeField] float IncomeMultipler = 1.75f;
    [SerializeField] Summoner Summoner; //no clue if that's the right class

    public void ScaleCost()
    {
        float income = Summoner.CalculateIncome();
        print("scaling cost: income is " + income);
        ManaDrain = RoundToHalf(income * IncomeMultipler);
    }

    private float RoundToHalf(float input)
    {
        float whole = (int)input;
        float frac = input - whole;
        int tens = (int)(10 * frac);
        if (tens <= 2.5)
        {
            return whole;
        } 
        if (tens >= 7.5)
        {
            return whole + 1;
        }
        return whole + 0.5f;
    }

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

        TileType under = MapManager.GetTileType(point);
        if (MapNode.IsTileOre(under))
        {
            return false; 
        }

        if (BlueprintBarracks.IsPointSpawnPoint(VectorRounder.RoundVectorToInt(point)))
        {
            return false; 
        }

        BlueprintBarracks bb;
        if (Summon.TryGetComponent<BlueprintBarracks>(out bb))
        {
            Vector2 spawnPoint = point + bb.GetSpawnOffset();
            TileType t = MapManager.GetTileType(spawnPoint);
            if (!MapManager.IsPointTraversable(spawnPoint, false) || MapNode.IsTileTypeBuilding(t))
            {
                return false; 
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