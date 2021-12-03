using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCoatingSelf : MonoBehaviour
{
    [SerializeField] CoatingManager CoatingManager;

    private void Update()
    {
        if (StandingOnSand())
        {
            CoatingManager.SetCoating(Coating.GetCoating(CoatingType.Water, 0.2f));
        }
    }

    bool StandingOnSand()
    {
        if (MapManager.GetTileType(VectorRounder.RoundVector(transform.position)) == TileType.Water)
        {
            return true;
        }
        return false;
    }
}
