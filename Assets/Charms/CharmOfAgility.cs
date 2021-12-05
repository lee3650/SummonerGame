using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfAgility : Charm
{
    [SerializeField] int DodgeProbability = 25;

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
            case EventType.Fire:
                if (Random.Range(0, 100) < DodgeProbability)
                {
                    return new Event(EventType.Dodge, 1f, e.Sender);
                }
                break;
        }
        return e;
    }
}
