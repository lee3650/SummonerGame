using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfMasochism : Charm
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
            case EventType.Poison:
            case EventType.Magic:
            case EventType.Fire:
                if (e.Sender != null)
                {
                    if (e.Sender.GetTransform() != null)
                    {
                        print("sender had transform!");
                        print("Sender transform name: " + e.Sender.GetTransform().name);

                        ITargetable target;
                        if (e.Sender.GetTransform().TryGetComponent<ITargetable>(out target))
                        {
                            if (target.CanBeTargetedBy(Factions.Nonplayer))
                            {
                                print("healing!");
                                return new Event(EventType.Heal, e.Magnitude, e.Sender);
                            }
                        }
                    }
                }

                break;
        }

        return e;
    }
}
