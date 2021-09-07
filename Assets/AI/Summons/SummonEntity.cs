using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEntity : AIEntity
{
    [SerializeField] Summon summon;
    [SerializeField] PointToHoldManager PointToHoldManager;

    public override void HandleEvent(Event e)
    {
        e = summon.GetCharmModifiedEvent(e);

        base.HandleEvent(e);
    }
}
