using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSummonAttackState : RangedAttackState, IDamager, IControllableState
{
    List<Event> EventModifiers = new List<Event>();
    public void AddAttackModifier(Event e)
    {
        EventModifiers.Add(e);
    }
    public void HandleCommand(PlayerCommand command)
    {

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
