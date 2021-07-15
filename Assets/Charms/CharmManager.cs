using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmManager : MonoBehaviour
{
    List<Charm> Charms = new List<Charm>(); 

    public Event GetCharmModifiedEvent(Event e, SummonType t)
    {
        foreach (Charm c in Charms)
        {
            if (c.ApplyToType(t))
            {
                e = c.GetCharmModifiedEvent(e);
            }
        }

        return e; 
    }

    public void AddCharm(Charm charm)
    {
        Charms.Add(charm);
    }

    public List<Charm> GetCharms()
    {
        return Charms; 
    }
}
