using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEntity : AIEntity
{
    const float HealthModifierPerTile = 0.075f;

    [SerializeField] Summon summon;
    [SerializeField] PointToHoldManager PointToHoldManager;

    public override void HandleEvent(Event e)
    {
        e = summon.GetCharmModifiedEvent(e);
        //e = GetAdjacentModifiedEvent(e);
        
        base.HandleEvent(e);
    }

    /*
    private Event GetAdjacentModifiedEvent(Event e)
    {
        int adjacentImpassableTiles = MapManager.GetNumOfAdjacentImpassableTiles(Mathf.RoundToInt(PointToHoldManager.PointToHold.x), Mathf.RoundToInt(PointToHoldManager.PointToHold.y));
        float damageModifier = 1 - (HealthModifierPerTile * adjacentImpassableTiles);
        print("Getting adjacent modified event whatever that does");

        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                return new Event(e.MyType, e.Magnitude * damageModifier, this);
        }

        return e; 
    }
     */
}
