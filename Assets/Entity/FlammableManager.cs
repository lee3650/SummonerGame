using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableManager : MonoBehaviour, IResettable
{
    private static List<ITargetable> flammables = new List<ITargetable>();

    void Awake()
    {
        ResetState();
    }

    public static void AddFlammable(ITargetable flammable)
    {
        flammables.Add(flammable);
    }

    public static List<ITargetable> GetFlammablesInRange(Vector2 position, float range, Factions targetedBy)
    {
        List<ITargetable> result = new List<ITargetable>();

        foreach (ITargetable t in flammables)
        {
            if (t != null && t.IsAlive())
            {
                if (t.CanBeTargetedBy(targetedBy) && Vector2.Distance(position, t.GetPosition()) < range) {
                    result.Add(t);
                }
            }
        }

        return result; 
    }

    public static void RemoveFlammable(ITargetable target)
    {
        flammables.Remove(target);
    }

    public void ResetState()
    {
        flammables = new List<ITargetable>();
    }
}
