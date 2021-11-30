using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfAgility : Charm
{
    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(EventType.Physical, 0f, sender);
    }

    public override Event GetCharmModifiedEvent(Event e)
    {
        switch (e.MyType)
        {
            case EventType.Physical:
            case EventType.Magic:
                if (Random.Range(0, 100) < 25)
                {
                    return new Event(e.MyType, 0f, e.Sender);
                }
                break;
        }
        return e;
    }
}
