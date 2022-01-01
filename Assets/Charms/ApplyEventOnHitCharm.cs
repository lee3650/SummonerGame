using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEventOnHitCharm : Charm
{
    [SerializeField] EventType EventType;
    [SerializeField] float Magnitude;
    [SerializeField] int Spreads;
    [SerializeField] int Recurrences;

    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(EventType.Physical, 0f, sender);
    }

    public override Event GetCharmModifiedEvent(Event e)
    {
        switch (e.MyType)
        {
            case EventType.Physical:
            case EventType.Fire:
            case EventType.Magic:
                if (e.Sender != null)
                {
                    e.Sender.HandleEvent(new Event(EventType, Magnitude, null, Recurrences, Spreads));
                }
                break;
        }
        return e; 
    }
}
