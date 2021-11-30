using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRecurringEvent : MonoBehaviour, ISubEntity
{
    [SerializeField] HealthManager hm;

    IEntity Entity;
    List<Event> RecurringEvents = new List<Event>();

    private void Awake()
    {
        if (hm == null)
        {
            hm = GetComponent<HealthManager>();
        }
        Entity = GetComponent<IEntity>();
    }

    public Event ModifyEvent(Event e)
    {
        return e;
    }

    public void HandleEvent(Event e)
    {
        print("Possibly handling event: " + e.ToString());

        if (e.Recurrences > 0 && hm.IsAlive())
        {
            print("Trying to recur event: " + e.ToString());

            if (!ContainsType(e))
            {
                print("Recurring event: " + e.ToString());
                RecurringEvents.Add(e);
                StartCoroutine(RecurEvent(e.MyType));
            } else
            {
                TryReplaceEvent(e);
            }
        } 
    }

    private void TryReplaceEvent(Event e)
    {
        for (int i = 0; i < RecurringEvents.Count; i++)
        {
            if (RecurringEvents[i].MyType == e.MyType && RecurringEvents[i].Recurrences < e.Recurrences)
            {
                RecurringEvents[i] = e;
            }
        }
    }

    private bool ContainsType(Event e)
    {
        Event r = GetEvent(e.MyType);
        return r != null;
    }

    private Event GetEvent(EventType type)
    {
        foreach (Event e in RecurringEvents)
        {
            if (e.MyType == type)
            {
                return e;
            }
        }
        return null;
    }

    IEnumerator RecurEvent(EventType type)
    {
        Event e = GetEvent(type);
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            if (hm.IsAlive() && e.Recurrences > 0)
            {
                Entity.HandleEvent(e);
            } else
            {
                break;
            }
            e.Recurrences--;
            e = GetEvent(type);
            print("Occurrences: " + e.Recurrences);
        }
    }

    public bool EventRecurs(EventType t)
    {
        Event e = GetEvent(t);
        if (e != null && e.Recurrences > 0)
        {
            return true;
        }
        return false; 
    }
}
