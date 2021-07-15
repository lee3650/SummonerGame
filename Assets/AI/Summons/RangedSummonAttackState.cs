using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSummonAttackState : RangedAttackState, IDamager
{
    List<Event> EventModifiers = new List<Event>();
    public void AddAttackModifier(Event e)
    {
        EventModifiers.Add(e);
    }

    protected override void ActivateProjectile(Projectile p)
    {
        base.ActivateProjectile(p);
        foreach (Event e in EventModifiers)
        {
            p.AddAttackModifier(e);
        }
    }
}
