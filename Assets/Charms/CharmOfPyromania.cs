using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfPyromania : Charm
{
    public override Event GetCharmModifiedEvent(Event e)
    {
        if (e.MyType == EventType.Fire)
        {
            return new Event(EventType.Heal, e.Magnitude, e.Sender);
        }
        return e;
    }

    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(EventType.Physical, 0f, sender);
    }
}
