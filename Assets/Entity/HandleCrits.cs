using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCrits : MonoBehaviour, ISubEntity
{
    [SerializeField] HandleRecurringEvent HandleRecurringEvent;

    public Event ModifyEvent(Event e)
    {
        print("Trying to convert " + e.MyType + " to crit!");

        bool convertToDamage = false;

        if (e.MyType == EventType.CritFire && HandleRecurringEvent.EventRecurs(EventType.Fire))
        {
            convertToDamage = true;
        }

        if (e.MyType == EventType.CritPoison && HandleRecurringEvent.EventRecurs(EventType.Poison))
        {
            convertToDamage = true;
        }

        if (convertToDamage)
        {
            print("converted to damage!");
            CritGraphicPool.ShowCrit((Vector2)transform.position + new Vector2(0, 1));
            return new Event(EventType.Physical, e.Magnitude, e.Sender);
        } else
        {
            print("did not convert to damage!");
        }

        return e;
    }

    public void HandleEvent(Event e)
    {
        //do nothin
    }
}
