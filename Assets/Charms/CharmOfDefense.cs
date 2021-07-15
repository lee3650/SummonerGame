using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfDefense : Charm
{
    public override Event GetAttackModifier()
    {
        return new Event(EventType.Physical, 10f);
    }

    public override Event GetCharmModifiedEvent(Event e)
    {
        return new Event(e.MyType, e.Magnitude * -1);
    }
}
