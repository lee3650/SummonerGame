using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfDefense : Charm
{
    [SerializeField] float DamageReduction = 0.75f;
    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(EventType.Physical, 0f, sender);
    }

    public override Event GetCharmModifiedEvent(Event e)
    {
        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                return new Event(e.MyType, e.Magnitude * DamageReduction, e.Sender);
        }
        return e; 
    }
}
