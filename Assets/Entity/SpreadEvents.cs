using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEvents : MonoBehaviour, ISubEntity
{
    [SerializeField] Factions TargetedBy;
    const float SpreadRange = 2f;

    public Event ModifyEvent(Event e)
    {
        return e;
    }

    public void HandleEvent(Event e)
    {
        if (e.Spreads <= 0)
        {
            return;
        }

        print("trying to spread " + e.MyType);

        List<ITargetable> targets = FlammableManager.GetFlammablesInRange(transform.position, SpreadRange, TargetedBy);

        print("Targets: " + targets.Count);

        foreach (ITargetable t in targets)
        {
            if (e.Spreads <= 0)
            {
                print("no spreads left!");
                return;
            }

            if (t != GetComponent<ITargetable>())
            {
                HandleRecurringEvent recur = t.GetTransform().GetComponent<HandleRecurringEvent>();

                bool apply = false;
                
                if (recur != null)
                {
                    if (!recur.EventRecurs(e.MyType))
                    {
                        apply = true;
                    }
                } else
                {
                    apply = true;
                }

                if (apply)
                {
                    print("apply event to " + e);

                    e.Spreads--;
                    t.HandleEvent(Event.CopyEvent(e));
                }
            }
        }
    }
}
