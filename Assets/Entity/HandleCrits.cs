using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCrits : MonoBehaviour, ISubEntity
{
    [SerializeField] HandleRecurringEvent HandleRecurringEvent;

    public Event ModifyEvent(Event e)
    {
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
            return new Event(EventType.Physical, e.Magnitude, e.Sender);
        }

        return e;
    }

    public void HandleEvent(Event e)
    {
        //do nothin
    }
}
